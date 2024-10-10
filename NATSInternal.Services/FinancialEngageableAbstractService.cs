namespace NATSInternal.Services;

internal abstract class FinancialEngageableAbstractService<
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
    : UpsertableAbstractService<
        T,
        TListRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
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

    protected FinancialEngageableAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IMonthYearService<T, TUpdateHistory> monthYearService)
    {
        _context = context;
        _authorizationService = authorizationService;
        _monthYearService = monthYearService;
    }

    /// <inheritdoc />
    protected override async Task<TListResponseDto> GetListAsync(
        IQueryable<T> query,
        TListRequestDto requestDto)
    {
        // Initialize month years options.
        List<MonthYearResponseDto> monthYearOptions = await _monthYearService
            .GenerateMonthYearOptions(GetRepository);

        TListResponseDto responseDto = await base.GetListAsync(query, requestDto);
        responseDto.MonthYearOptions = monthYearOptions;

        return responseDto;
    }
    
    /// <inheritdoc />
    protected override async Task<TDetailResponseDto> GetDetailAsync(IQueryable<T> query)
    {
        // Determine if the update histories should be fetched.
        bool shouldIncludeUpdateHistories = CanAccessUpdateHistory(_authorizationService);
        if (shouldIncludeUpdateHistories)
        {
            query = query.Include(d => d.UpdateHistories);
        }

        // Fetch the entity with the given id and ensure it exists in the database.
        T entity = await query.SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();

        return InitializeDetailResponseDto(
            entity,
            _authorizationService,
            shouldIncludeUpdateHistories);
    }

    /// <summary>
    /// Creates a new entity with the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is the id of
    /// the new entity.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the value for the <c>StatsDateTime</c> property has been provided in the
    /// <c>requestDto</c>, but the requesting user doesn't have enough permissions to do so.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when a requesting-user-related conflict or when a payee-related conflict occurs
    /// during the operation.
    /// </exception>
    public virtual async Task<int> CreateAsync(TUpsertRequestDto requestDto)
    {
        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine the value for StatsDateTime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Check if the current user has permission to specify a value for PaidDateTime.
            if (!_authorizationService.CanSetExpensePaidDateTime())
            {
                throw new AuthorizationException();
            }

            statsDateTime = requestDto.StatsDateTime.Value;
        }

        // Initialize entity.
        T entity = new T
        {
            StatsDateTime = statsDateTime,
            Note = requestDto.Note,
            CreatedUserId = _authorizationService.GetUserId(),
        };

        GetRepository(_context).Add(entity);

        CustomizeEntityCreatingPostAssignment(entity, requestDto);

        try
        {
            await _context.SaveChangesAsync();

            // The entity can be created successfully, adjust the stats.
            await IncrementStatsAsync(_statsService, entity);

            // Commit the transaction, finishing the operation.
            await transaction.CommitAsync();
            return entity.Id;
        }
        catch (DbUpdateException exception)
        {
            HandleCreatingOperationException(exception);
            throw;
        }
    }

    /// <summary>
    /// Customizes the post-data-assignment process from the request DTO to the new initialized
    /// entity in the creating operation.
    /// </summary>
    /// <remarks>
    /// This method is called immediately right after the new entity has been initialized.
    /// Override this method to provide the post-assignment customization.
    /// </remarks>
    /// <param name="entity">
    /// The entity of which properties data assignment are to be customized.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the new entity creating operation.
    /// </param>
    protected virtual void CustomizeEntityCreatingPostAssignment(
            T entity,
            TUpsertRequestDto requestDto) { }

    /// <inheritdoc />
    protected sealed override TBasicResponseDto InitializeBasicResponseDto(T entity)
    {
        return InitializeBasicResponseDto(entity, _authorizationService);
    }

    /// <inheritdoc />
    protected sealed override TDetailResponseDto InitializeDetailResponseDto(T entity)
    {
        return InitializeDetailResponseDto(entity, _authorizationService, false);
    }

    /// <summary>
    /// Gets the entity repository in the <see cref="DatabaseContext"/> class.
    /// <param name="context">
    /// An instance of the injected <see cref="DatabaseContext"/>
    /// </param>
    /// <returns>The entity repository.</returns>
    protected abstract DbSet<T> GetRepository(DatabaseContext context);

    /// <summary>
    /// Initializes a response DTO, contanining the basic information of the given entity.
    /// </summary>
    /// <param name="entity">
    /// The entity to map to the DTO.
    /// </param>
    /// <param name="service">
    /// The service providing the authorization information.
    /// </param>
    /// <returns>
    /// The initialized DTO.
    /// </returns>
    protected abstract TBasicResponseDto InitializeBasicResponseDto(
            T entity,
            IAuthorizationInternalService service);

    /// <summary>
    /// Initializes a response DTO, contanining the details of the given entity.
    /// </summary>
    /// <param name="entity">
    /// The entity to map to the DTO.
    /// </param>
    /// <param name="shouldIncludeUpdateHistories">
    /// Indicates that the associated update history DTOs should also be included in the detail
    /// response DTO.
    /// </param>
    /// <returns>
    /// The initialized DTO.
    /// </returns>
    protected abstract TDetailResponseDto InitializeDetailResponseDto(
            T entity,
            IAuthorizationInternalService service,
            bool shouldIncludeUpdateHistories);

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
    /// <param name="service">
    /// The service providing the authorization information.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing the permission.
    /// </returns>
    protected abstract bool CanAccessUpdateHistory(IAuthorizationInternalService service);

    /// <summary>
    /// Determines whether the current user has enough permissions to set a value for the
    /// <c>StatsDateTime</c> property in the entity, used in the creating or updating
    /// operation.
    /// </summary>
    /// <param name="service">
    /// The service providing the authorization information.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing the permission.
    /// </returns>
    protected abstract bool CanSetStatsDateTime(IAuthorizationInternalService service);

    /// <summary>
    /// Determines whether the current user has enough permissions to edit the specified
    /// entity, used in the creating or updating operation.
    /// </summary>
    /// <param name="service">
    /// The service providing the authorization information.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing the permission.
    /// </returns>
    protected abstract bool CanEdit(T entity, IAuthorizationInternalService service);

    /// <summary>
    /// Determines whether the current user has enough permissions to delete a specific entity,
    /// used in the deleting operation.
    /// </summary>
    /// <param name="service">
    /// The service providing the authorization information.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing the permission.
    /// </returns>
    protected abstract bool CanDelete(IAuthorizationInternalService service);

    /// <summary>
    /// Increment the statistics amount based on the specified <see cref="T"/> entity.
    /// </summary>
    /// <param name="service">
    /// An instance of the service to handle the statistics amount incrementing operation.
    /// </param>
    /// <param name="entity">
    /// The entity to which the incremnenting statistics amount belongs.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected abstract Task IncrementStatsAsync(
            IStatsInternalService<T, TUpdateHistory> service,
            T entity);

    /// <summary>
    /// Handles the exception thrown by the database during the creating operation.
    /// </summary>
    /// <param name="exception">
    /// The exception instance thrown by the database.
    /// </param>
    protected abstract void HandleCreatingOperationException(DbUpdateException exception);
}