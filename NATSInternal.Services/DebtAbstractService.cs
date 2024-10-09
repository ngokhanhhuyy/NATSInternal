using Microsoft.EntityFrameworkCore.Query;

namespace NATSInternal.Services;

/// <summary>
/// An abstract service to handle both debt incurrence and debt payment related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the debt entity.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity associated to the <see cref="T"/> entity.
/// </typeparam>
/// <typeparam name="TListRequestDto">
/// The type of the request DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TUpsertRequestDto">
/// The type of the request DTO used in the creating or updating operation.
/// </typeparam>
/// <typeparam name="TListResponseDto">
/// The type of the response DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TBasicResponseDto">
/// The type of the response DTO, containing the basic information of the <see cref="T"/>
/// entity in the list retriving operation as individual items.
/// </typeparam>
/// <typeparam name="TDetailResponseDto">
/// The type of the response DTO, containing the details of the <see cref="T"/> entity in the
/// detail retriving operation.
/// </typeparam>
/// <typeparam name="TUpdateHistoryResponseDto">
/// The type of the update history response dto, containing the data of the updating history
/// entity associated to the <see cref="T"/> entity.
/// </typeparam>
/// <typeparam name="TUpdateHistoryDataDto">
/// The type of the update history data DTO, containing the data of a specific <see cref="T"/>
/// entity instance after each modification, used in the updating operation.
/// </typeparam>
/// <typeparam name="TListAuthorizationResponseDto">
/// The type of the response DTO, containing the authorization information for the
/// <see cref="TListResponseDto"/> DTO in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TAuthorizationResponseDto">
/// The type of the response DTO, containing the authorization information for the
/// <see cref="TBasicResponseDto"/> and <see cref="TDetailResponseDto"/> DTOs, used in the
/// list retrieving and detail retrieving operations.
/// </typeparam>
internal abstract class DebtAbstractService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TUpsertRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TUpdateHistoryResponseDto,
        TUpdateHistoryDataDto,
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
    where TDetailResponseDto : IDebtDetailResponseDto<
        TUpdateHistoryResponseDto,
        TAuthorizationResponseDto>
    where TUpdateHistoryResponseDto : IUpdateHistoryResponseDto
    where TListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
    where TAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService<T, TUpdateHistory> _statsService;
    private readonly IUpdateHistoryService<T, TUpdateHistory, TUpdateHistoryDataDto> _updateHistoryService;
    private readonly IMonthYearService<T, TUpdateHistory> _monthYearService;
    private readonly DebtType _debtType;

    protected DebtAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<T, TUpdateHistory> statsService,
            IUpdateHistoryService<T, TUpdateHistory, TUpdateHistoryDataDto> updateHistoryService,
            IMonthYearService<T, TUpdateHistory> monthYearService,
            DebtType debtType)
    {
        _context = context;
        _authorizationService = authorizationService;
        _statsService = statsService;
        _updateHistoryService = updateHistoryService;
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

    /// <summary>
    /// Retrieves a list of DTOs containing the basic information of the debt instances,
    /// specified filtering, sorting and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="DebtPaymentListRequestDto"/> class, containing the
    /// conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the results of the operation and the additional
    /// information for pagination.
    /// </returns>
    public virtual async Task<TListResponseDto> GetListAsync(TListRequestDto requestDto)
    {
        // Generate month year options.
        List<MonthYearResponseDto> monthYearOptions = await _monthYearService
            .GenerateMonthYearOptions(GetRepository);

        // Initialize query.
        IQueryable<T> query = InitializeListQuery(requestDto);

        // Initialize response dto.
        TListResponseDto responseDto = new TListResponseDto
        {
            MonthYearOptions = monthYearOptions,
            Authorization = InitializeListAuthorizationResponseDto(_authorizationService)
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

    /// <summary>
    /// Retrieves the details of a specific debt entity by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the debt entity to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO
    /// containing the details of the debt payment.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the debt entity with the specified <c>id</c> arguments doesn't exist or
    /// has already been deleted.
    /// </exception>
    public virtual async Task<TDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<T> query = GetRepository(_context)
            .Include(d => d.Customer)
            .Include(d => d.CreatedUser).ThenInclude(u => u.Roles);

        // Determine if the update histories should be fetched.
        bool shouldIncludeUpdateHistories = CanAccessUpdateHistory(_authorizationService);
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

    /// <summary>
    /// Creates a new debt entity based on the specified data from the request.
    /// </summary>
    /// <param name="requestDto">
    /// An DTO containing the data for the creating operation.
    /// </param>
    /// <param name="entityInitializer">
    /// (Optional) An expression which is called immediately after the initialization of the
    /// new entity, used to customize the data assignment to the properties of the entity.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asychronous operation, which result is the id of
    /// the new debt entity.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to specify a value
    /// for the <c>StatsDateTime</c> property in the <c>requestDto</c> argument.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when the information of the requesting user has already been deleted before the
    /// operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - The customer specified by the <c>CustomerId</c> in the <c>requestDto</c> argument
    /// doesn't exist or has already been deleted.
    /// - The remaining debt amount of the specified customer becomes negative after the
    /// operation.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Throws the <c>_debtType</c> field has a value which handling logic hasn't been
    /// implemented yet.
    /// </exception>
    public virtual async Task<int> CreateAsync(TUpsertRequestDto requestDto)
    {
        // Determining the stats datetime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the current user has permission to specify a value for StatsDateTime.
            if (CanSetStatsDateTime(_authorizationService))
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

        CustomizeEntityInitialization(entity, requestDto);

        GetRepository(_context).Add(entity);
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync();
            
            // The entity is saved successfully, adjust the stats based on the debt type.
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
                    throw new NotImplementedException(errorMessage);
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
    /// Updates an existing debt entity, based on its id and the provided data.
    /// </summary>
    /// <param name="id">
    /// The id of the debt payment to update.
    /// </param>
    /// <param name="requestDto">
    /// A DTO, containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the debt payment specified by the <c>id</c> argument doesn't exist or has
    /// already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws under the following circumstances:<br/>
    /// - When the requesting user doesn't have enough permissions to update the debt payment.
    /// - When the requesting user can update the debt payment, but doesn't have enough
    /// permissions to specify a value for the <c>SupplyDateTime</c> property in the
    /// <c>requestDto</c> argument.
    /// </exception>
    /// <exception cref="ValidationException">
    /// Throws when the value of the <c>SupplyDateTime</c> property in the <c>requestDto</c>
    /// argument is invalid.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the <c>SupplyDateTime</c> property in the <c>requestDto</c> argument is
    /// specified a value while the debt payment has already been locked.
    /// - When the remaining debt amount of the associated customer becomes negative after the
    /// operation.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws under the following circumstances:<br/>
    /// - When the debt payment has been modified by another process before the operation.<br/>
    /// - When the information of the requesting user has been deleted by another process
    /// before the operation.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Throws the <c>_debtType</c> field has a value which handling logic hasn't been
    /// implemented yet.
    /// </exception>
    public virtual async Task UpdateAsync(
            int id,
            TUpsertRequestDto requestDto)
    {
        // Prepare the query to fetch the entity.
        IQueryable<T> query = GetRepository(_context)
            .Include(e => e.Customer).ThenInclude(c => c.DebtIncurrences)
            .Include(e => e.Customer).ThenInclude(c => c.DebtPayments)
            .Include(d => d.CreatedUser);

        // Filter by id and soft deleted.
        query = query.Where(dp => dp.Id == id && !dp.IsDeleted);

        // Fetch the entity.
        T entity = await query.SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();
        
        // Check if the current user has permission to edit the debt payment.
        if (!InitializeAuthorizationResponseDto(_authorizationService, entity).CanEdit)
        {
            throw new AuthorizationException();
        }
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Store the old data and create new data for stats adjustment.
        long oldAmount = entity.Amount;
        DateOnly oldPaidDate = DateOnly.FromDateTime(entity.StatsDateTime);
        TUpdateHistoryDataDto oldData = InitializeUpdateHistoryDataDto(entity);

        // Update the paid datetime if specified.
        if (requestDto.StatsDateTime.HasValue)
        {
            // Check if the current user has permission to change the created datetime.
            if (!_authorizationService.CanSetDebtIncurredDateTime())
            {
                throw new AuthorizationException();
            }

            // Prevent StatsDateTime to be modified when the entity is already locked.
            if (entity.IsLocked)
            {
                string errorMessage = ErrorMessages.CannotSetDateTimeAfterLocked
                    .ReplaceResourceName(DisplayNames.Get(typeof(T).Name))
                    .ReplacePropertyName(DisplayNames.Get(entity.GetStatsPropertyName()));
                throw new OperationException(
                    entity.GetStatsPropertyName(),
                    errorMessage);
            }
            
            // Assign the new StatsDateTime value only if it's different from the old one.
            if (requestDto.StatsDateTime.Value != entity.StatsDateTime)
            {
                // Verify if the amount has been changed, and with the new amount, the remaning
                // debt amount won't be negative.
                if (requestDto.Amount != entity.Amount)
                {
                    entity.Amount = requestDto.Amount;
                    if (entity.Customer.DebtAmount < 0)
                    {
                        throw new OperationException(
                            nameof(requestDto.Amount),
                            ErrorMessages.NegativeRemainingDebtAmount);
                    }
                }
                
                // Validate the specified SupplyDateTime from the request.
                try
                {
                    _statsService.ValidateStatsDateTime(
                        entity,
                        requestDto.StatsDateTime.Value);
                }
                catch (ValidationException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.Get(entity.GetStatsPropertyName()));
                    throw new OperationException(
                        entity.GetStatsPropertyName(),
                        errorMessage);
                }

                // The specified StatsDateTime is valid, assign it to the entity.
                entity.StatsDateTime = requestDto.StatsDateTime.Value;
            }
        }

        // Verify that with the new paid amount, the customer's remaining debt amount will
        // not be negative.
        if (entity.Customer.DebtAmount < requestDto.Amount)
        {
            const string amountErrorMessage = ErrorMessages.NegativeRemainingDebtAmount;
            throw new OperationException(nameof(requestDto.Amount), amountErrorMessage);
        }
        
        // Update other properties.
        entity.Amount = requestDto.Amount;
        entity.Note = requestDto.Note;
        
        // Store new data for update history logging.
        TUpdateHistoryDataDto newData = InitializeUpdateHistoryDataDto(entity);
        
        // Log update history.
        _updateHistoryService
            .LogUpdateHistory(entity, oldData, newData, requestDto.UpdatedReason);
        
        // Perform the update operations.
        try
        {
            await _context.SaveChangesAsync();
            
            // The debt payment can be updated successfully without any error.
            // Adjust the stats.
            // Revert the old stats.
            long oldStatsAmount;
            long newStatsAmount;
            switch (_debtType)
            {
                case DebtType.DebtIncurrence:
                    oldStatsAmount = oldAmount;
                    newStatsAmount = -entity.Amount;
                    break;
                case DebtType.DebtPayment:
                    oldStatsAmount = -oldAmount;
                    newStatsAmount = entity.Amount;
                    break;
                default:
                    string errorMessage = $"[{_debtType}] is not a supported value.";
                    throw new NotImplementedException(errorMessage);
            }
            await _statsService.IncrementDebtAmountAsync(oldAmount, oldPaidDate);
            
            // Add new stats.
            DateOnly newStatsDate = DateOnly.FromDateTime(entity.StatsDateTime);
            await _statsService.IncrementDebtAmountAsync(newStatsAmount, newStatsDate);
            
            // Commit the transaction and finish the operation.
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            // Handling concurrecy exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }
            
            // Handling data exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                HandleCreateOrUpdateException(sqlException);
            }
            
            throw;
        }
    }

    /// <summary>
    /// Initializes the query for list retrieving operation, based on the filtering, sorting
    /// and paginating conditions specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">A DTO containing the conditions for the results.</param>
    /// <returns>A query instance used to perform the list retrieving operation.</returns>
    protected virtual IQueryable<T> InitializeListQuery(TListRequestDto requestDto)
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

    /// <summary>
    /// Handles the exception thrown by the database during the creating or updating operation.
    /// </summary>
    /// <param name="exception">
    /// The exception thrown by the database during the operation.
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

    /// <summary>
    /// Customizes the data assignment from the request DTO to the new initialized entity.
    /// </summary>
    /// <remarks>
    /// This method is called immediately right after the new entity has been initialized.
    /// Override this method to provide the initialization customization.
    /// </remarks>
    /// <param name="entity">
    /// The entity of which properties data assignment are to be customized.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the new entity creating operation.
    /// </param>
    protected virtual void CustomizeEntityInitialization(
            T entity,
            TUpsertRequestDto requestDto) { }

    /// <summary>
    /// Gets the entity repository in the <see cref="DatabaseContext"/> class.
    /// <param name="context">
    /// An instance of the injected <see cref="DatabaseContext"/>
    /// </param>
    /// <returns>The entity repository.</returns>
    protected abstract DbSet<T> GetRepository(DatabaseContext context);
    
    /// <summary>
    /// Provides the sorting conditions for the list retrieving operation, based on the
    /// specified conditions.
    /// </summary>
    /// <param name="query">An initialized query instance.</param>
    /// <param name="requestDto">The DTO containing the conditions for sorting.</param>
    /// <returns>A sorted query used for list retrieving operation.</returns>
    protected abstract IOrderedQueryable<T> SortListQuery(
            IQueryable<T> query,
            TListRequestDto requestDto);

    /// <summary>
    /// Provides the filtering conditions by month and year, based on the specified conditions.
    /// </summary>
    /// <param name="query">
    /// An initialized query instnace.
    /// </param>
    /// <param name="minimumDateTime">
    /// The minimum <see cref="DateTime"/> value that the value of the <c>StatsDateTime</c>
    /// in the entity has to be greater than to fulfill the condition.
    /// </param>
    /// <param name="maximumDateTime">
    /// The maximum <see cref="DateTime"/> value that the value of the <c>StatsDateTime</c>
    /// in the entity has to be greater than to fulfill the condition.</param>
    /// <returns></returns>
    protected abstract IQueryable<T> FilterByMonthYearListQuery(
            IQueryable<T> query,
            DateTime minimumDateTime,
            DateTime maximumDateTime);

    /// <summary>
    /// Initializes a response DTO, contanining the basic information of the given entity.
    /// </summary>
    /// <param name="entity">The entity to map to the DTO.</param>
    /// <returns>The initialized DTO.</returns>
    protected abstract TBasicResponseDto InitializeBasicResponseDto(T entity);

    /// <summary>
    /// Initializes a response DTO, contanining the details of the given entity.
    /// </summary>
    /// <param name="entity">The entity to map to the DTO.</param>
    /// <param name="shouldIncludeUpdateHistories">
    /// Indicates that the associated update history DTOs should also be included in the detail
    /// response DTO.
    /// </param>
    /// <returns>The initialized DTO.</returns>
    protected abstract TDetailResponseDto InitializeDetailResponseDto(
            T entity,
            bool shouldIncludeUpdateHistories);

    /// <summary>
    /// Initializes a response DTO, containing the authorization information for the list
    /// response DTO, used in the list retrieving operation.
    /// </summary>
    /// <param name="authorizationService">
    /// The authorization service to retrieve the authorization information.
    /// </param>
    /// <returns>The initialized DTO.</returns>
    protected abstract TListAuthorizationResponseDto InitializeListAuthorizationResponseDto(
            IAuthorizationInternalService authorizationService);

    /// <summary>
    /// Initializes a response DTO, containing the authorization information of a specified
    /// entity for the basic and the detail response DTO, used in the list retrieving and
    /// detail retriving operation.
    /// </summary>
    /// <param name="authorizationService">
    /// The authorization service to retrieve the authorization information.
    /// </param>
    /// <param name="entity">
    /// The entity to retrive the authorization for.
    /// </param>
    /// <returns>The initialized DTO.</returns>
    protected abstract TAuthorizationResponseDto InitializeAuthorizationResponseDto(
            IAuthorizationInternalService authorizationService,
            T entity);

    /// <summary>
    /// Initializes an update history data DTO, containing the data of the specified entity
    /// at the called time, used for storing the data before and after modifications in the
    /// updating operation.
    /// </summary>
    /// <param name="entity">The entity which data is to be stored.</param>
    /// <returns>The intialized DTO.</returns>
    protected abstract TUpdateHistoryDataDto InitializeUpdateHistoryDataDto(T entity);

    /// <summary>
    /// Determines whether the current user has enough permissions to access the update history
    /// of any entity, used in the detail retrieving operation.
    /// </summary>
    /// <param name="service">The service providing the authorization information.</param>
    /// <returns>A <see cref="bool"/> value representing the permission.</returns>
    protected abstract bool CanAccessUpdateHistory(IAuthorizationInternalService service);

    /// <summary>
    /// Determines whether the current user has enough permissions to set a value for the
    /// <c>StatsDateTime</c> property in the entity, used in the creating or updating
    /// operation.
    /// </summary>
    /// <param name="service">The service providing the authorization information.</param>
    /// <returns>A <see cref="bool"/> value representing the permission.</returns>
    protected abstract bool CanSetStatsDateTime(IAuthorizationInternalService service);
}