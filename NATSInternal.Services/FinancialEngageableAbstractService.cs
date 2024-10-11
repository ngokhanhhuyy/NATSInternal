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
    private readonly IMonthYearService<T, TUpdateHistory> _monthYearService;

    protected FinancialEngageableAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IMonthYearService<T, TUpdateHistory> monthYearService)
        : base(authorizationService)
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
    /// Initializes the query for list retrieving operation, based on the filtering, sorting
    /// and paginating conditions specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A query instance used to perform the list retrieving operation.
    /// </returns>
    protected virtual IQueryable<T> InitializeListQuery(TListRequestDto requestDto)
    {
        IQueryable<T> query = GetRepository(_context)

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
    /// Logs the old and new data to update history for the specified entity.
    /// </summary>
    /// <param name="entity">
    /// An instance of the <see cref="T"/> entity class, representing the entity to be logged.
    /// </param>
    /// <param name="oldData">
    /// An instance of the <see cref="TUpdateHistoryDataDto"/> class, containing the data of
    /// the entity before the modification.
    /// </param>
    /// <param name="newData">
    /// An instance of the <see cref="TUpdateHistoryDataDto"/> class, containing the data of
    /// the entity after the modification.
    /// </param>
    /// <param name="reason">
    /// The reason of the modification.
    /// </param>
    public void LogUpdateHistory(
            T entity,
            TUpdateHistoryDataDto oldData,
            TUpdateHistoryDataDto newData,
            string reason)
    {
        TUpdateHistory updateHistory = new TUpdateHistory
        {
            Reason = reason,
            OldData = JsonSerializer.Serialize(oldData),
            NewData = JsonSerializer.Serialize(newData),
            UpdatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            UpdatedUserId = _authorizationService.GetUserId()
        };
        entity.UpdateHistories ??= new List<TUpdateHistory>();
        entity.UpdateHistories.Add(updateHistory);
    }

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
    /// <param name="query">
    /// An initialized query instance.
    /// </param>
    /// <param name="requestDto">
    /// The DTO containing the conditions for sorting.
    /// </param>
    /// <returns>
    /// A sorted query used for list retrieving operation.
    /// </returns>
    protected abstract IOrderedQueryable<T> SortListQuery(
            IQueryable<T> query,
            TListRequestDto requestDto);

    /// <summary>
    /// Provides the filtering conditions by month and year, based on the specified conditions.
    /// </summary>
    /// <param name="query">
    /// An initialized query instance.
    /// </param>
    /// <param name="minimumDateTime">
    /// The minimum <see cref="DateTime"/> value that the value of the <c>StatsDateTime</c>
    /// in the entity has to be greater than to fulfill the condition.
    /// </param>
    /// <param name="maximumDateTime">
    /// The maximum <see cref="DateTime"/> value that the value of the <c>StatsDateTime</c>
    /// in the entity has to be greater than to fulfill the condition.</param>
    /// <returns>
    /// The query instance after adding the filtering conditions.
    /// </returns>
    protected abstract IQueryable<T> FilterByMonthYearListQuery(
            IQueryable<T> query,
            DateTime minimumDateTime,
            DateTime maximumDateTime);

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
}