namespace NATSInternal.Services;

internal abstract class DebtService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TUpsertRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TUpdateHistoryResponseDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    where T : class, IDebtEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto :
        IFinancialEngageableListRequestDto,
        ICustomerEngageableListRequestDto
    where TUpsertRequestDto : ICustomerEngageableUpsertRequestDto
    where TListResponseDto :
        IFinancialEngageableListResponseDto<
            TBasicResponseDto,
            TAuthorizationResponseDto,
            TListAuthorizationResponseDto>,
        new()
    where TBasicResponseDto :
        class,
        ICustomerEngageableBasicResponseDto<TAuthorizationResponseDto>
    where TDetailResponseDto :
        ICustomerEngageableBasicResponseDto<TAuthorizationResponseDto>,
        IFinancialEngageableDetailResponseDto<
            TUpdateHistoryResponseDto,
            TAuthorizationResponseDto>
    where TUpdateHistoryResponseDto : IUpdateHistoryResponseDto
    where TListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
    where TAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    protected readonly DatabaseContext _context;
    protected readonly IAuthorizationInternalService _authorizationService;
    protected readonly IStatsInternalService<T, TUpdateHistory> _statsService;
    protected readonly IMonthYearService<T, TUpdateHistory> _monthYearService;
    protected readonly DebtType _debtType;

    protected DebtService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<T, TUpdateHistory> statsService,
            IMonthYearService<T, TUpdateHistory> monthYearService,
            DebtType debtType)
    {
        _context = context;
        _authorizationService = authorizationService;
        _statsService = statsService;
        _monthYearService = monthYearService;

        if (debtType != DebtType.DebtIncurrence && debtType!= DebtType.DebtPayment)
        {
            string errorMessage = $"[{_debtType}] is not a supported value. Please update " +
                "the logic in this class to add support for the specified value to avoid " +
                "any unexpected error.";
            throw new ArgumentException(errorMessage, nameof(debtType));
        }
        _debtType = debtType;
    }

    public virtual async Task<TListResponseDto> GetListAsync(TListRequestDto requestDto)
    {
        // Generate month year options.
        List<MonthYearResponseDto> monthYearOptions = await _monthYearService
            .GenerateMonthYearOptions(GetRepository);

        // Initialize query.
        IQueryable<T> query = GetListQuery(requestDto);

        // Initialize response dto.
        TListResponseDto responseDto = new TListResponseDto
        {
            MonthYearOptions = monthYearOptions,
            Authorization = GetListAuthorizationResponseDto()
        };

        int resultCount = await query.CountAsync();
        if (resultCount == 0)
        {
            responseDto.PageCount = 0;
            return responseDto;
        }
        
        responseDto.PageCount = (int)Math.Ceiling(
            (double)resultCount / requestDto.ResultsPerPage);

        List<T> entities = await query
            .Skip(requestDto.ResultsPerPage * (requestDto.Page - 1))
            .Take(requestDto.ResultsPerPage)
            .AsSplitQuery()
            .ToListAsync();
        responseDto.Items = entities.Select(InitializeBasicResponseDto).ToList();

        return responseDto;
    }

    protected virtual IQueryable<T> GetListQuery(TListRequestDto requestDto)
    {
        IQueryable<T> query = GetRepository(_context)
            .Include(entity => entity.Customer);

        // Sort by the specified direction and field.
        query = SortListQuery(query, requestDto);

        // Filter by month and year if specified.
        if (!requestDto.IgnoreMonthYear)
        {
            DateTime startingDateTime = new DateTime(requestDto.Year, requestDto.Month, 1);
            DateTime endingDateTime = startingDateTime.AddMonths(1);
            query = FilterByMonthYearListQuery(query, startingDateTime, endingDateTime);
        }

        // Filter by user id if specified.
        if (requestDto.CreatedUserId.HasValue)
        {
            query = query.Where(o => o.CreatedUserId == requestDto.CreatedUserId);
        }

        // Filter by customer id if specified.
        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(o => o.CustomerId == requestDto.CustomerId);
        }

        // Filter by not being soft deleted.
        query = query.Where(o => !o.IsDeleted);

        return query;
    }

    public virtual async Task<TDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<T> query = GetRepository(_context)
            .Include(d => d.Customer)
            .Include(d => d.CreatedUser).ThenInclude(u => u.Roles);

        // Determine if the update histories should be fetched.
        bool shouldIncludeUpdateHistories = CanAccessUpdateHistory();
        if (shouldIncludeUpdateHistories)
        {
            query = query.Include(d => d.UpdateHistories);
        }

        // Fetch the entity with the given id and ensure it exists in the database.
        T entity = await query
            .AsSplitQuery()
            .Where(d => d.Id == id)
            .Where(d => !d.IsDeleted)
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();

        return InitializeDetailResponseDto(entity, shouldIncludeUpdateHistories);
    }

    public virtual async Task<int> CreateAsync(TUpsertRequestDto requestDto)
    {
        // Determining the stats datetime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the current user has permission to specify a value for StatsDateTime.
            if (CanSetStatsDateTime())
            {
                throw new AuthorizationException();
            }

            statsDateTime = requestDto.StatsDateTime.Value;
        }
        
        // Initialize the entity.
        T entity = new T
        {
            Amount = requestDto.Amount,
            Note = requestDto.Note,
            StatsDateTime = statsDateTime,
            CustomerId = requestDto.CustomerId,
            CreatedUserId = _authorizationService.GetUserId()
        };
        GetRepository(_context).Add(entity);
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync();
            
            // The entity is saved successfully, adjust the stats.
            long statsAmount;
            switch (_debtType)
            {
                case DebtType.DebtIncurrence:
                    statsAmount = entity.Amount;
                    break;
                case DebtType.DebtPayment:
                    statsAmount = -entity.Amount;
                    break;
                default:
                    string errorMessage = $"[{_debtType}] is not a supported value.";
                    throw new InvalidDataException(errorMessage);
            }

            await _statsService.IncrementDebtAmountAsync(
                statsAmount,
                DateOnly.FromDateTime(entity.CreatedDateTime));
            
            // Commit the transaction, finish all operations.
            await transaction.CommitAsync();
            
            return entity.Id;
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlException)
        {
            HandleCreateOrUpdateException(sqlException);
            throw;
        }
    }

    /// <summary>
    /// Handles the exception thrown by the database during the creating or updating operation.
    /// </summary>
    /// <param name="exception">
    /// An instance of the <see cref="MySqlException"/> class, containing the details of the
    /// error.
    /// </param>
    /// <exception cref="OperationException">
    /// Throws when the <c>exception</c> indicates that the <c>CustomerId</c> foreign key
    /// references to a non-existent customer.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when the <c>exception</c> indicates that the information of the requesting user
    /// has been deleted before the operation.
    /// </exception>
    private static void HandleCreateOrUpdateException(MySqlException exception)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
        exceptionHandler.Handle(exception);
        if (exceptionHandler.IsForeignKeyNotFound)
        {
            switch (exceptionHandler.ViolatedFieldName)
            {
                // The foreign key CustomerId references to a non-existent customer entity.
                case "customer_id":
                    string errorMessage = ErrorMessages.NotFound
                        .ReplaceResourceName(DisplayNames.Customer);
                    throw new OperationException("customerId", errorMessage);
                
                // The foreign key CreatedUserId references to a user which might have been
                // deleted.
                default:
                    throw new ConcurrencyException();
            }
        }
    }

    protected abstract DbSet<T> GetRepository(DatabaseContext context);
    protected abstract IOrderedQueryable<T> SortListQuery(
            IQueryable<T> query,
            TListRequestDto requestDto);
    protected abstract IQueryable<T> FilterByMonthYearListQuery(
            IQueryable<T> query,
            DateTime startingDateTime,
            DateTime endingDateTime);
    protected abstract TBasicResponseDto InitializeBasicResponseDto(T entity);
    protected abstract TDetailResponseDto InitializeDetailResponseDto(
            T entity,
            bool shouldIncludeUpdateHistories);
    protected abstract TListAuthorizationResponseDto GetListAuthorizationResponseDto();
    protected abstract TAuthorizationResponseDto GetAuthorizationResponseDto(T entity);
    protected abstract bool CanAccessUpdateHistory();
    protected abstract bool CanSetStatsDateTime();
    protected abstract TAuthorizationResponseDto GetAuthorization();
}