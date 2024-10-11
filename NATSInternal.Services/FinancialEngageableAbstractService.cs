namespace NATSInternal.Services;

internal abstract class FinancialEngageableAbstractService<
        T,
        TUpdateHistory,
        TListRequestDto,
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
    where TListRequestDto : IFinancialEngageableListRequestDto
    where TListResponseDto :
        IFinancialEngageableListResponseDto<
            TBasicResponseDto,
            TAuthorizationResponseDto,
            TListAuthorizationResponseDto>,
        new()
    where TBasicResponseDto :
        class,
        IFinancialEngageableBasicResponseDto<TAuthorizationResponseDto>
    where TDetailResponseDto : IFinancialEngageableDetailResponseDto<
        TUpdateHistoryResponseDto,
        TAuthorizationResponseDto>
    where TUpdateHistoryResponseDto : IUpdateHistoryResponseDto
    where TListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
    where TAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;

    protected FinancialEngageableAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService)
        : base(context, authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public override async Task<TListResponseDto> GetListAsync(TListRequestDto requestDto)
    {
        // Initialize month years options.
        List<MonthYearResponseDto> monthYearOptions;
        monthYearOptions = await GenerateMonthYearOptions();

        TListResponseDto responseDto = await base.GetListAsync(requestDto);
        responseDto.MonthYearOptions = monthYearOptions;

        return responseDto;
    }

    /// <inheritdoc />
    protected override IQueryable<T> InitializeListQuery(TListRequestDto requestDto)
    {
        IQueryable<T> query = GetRepository(_context);

        // Sort by the specified direction and field.
        query = SortListQuery(query, requestDto.OrderByAscending, requestDto.OrderByField);

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

        // Filter by not being soft deleted.
        query = query.Where(o => !o.IsDeleted);

        return query;
    }

    /// <inheritdoc />
    protected override IQueryable<T> InitializeDetailQuery()
    {
        IQueryable<T> query = base.InitializeDetailQuery()
            .Include(e => e.CreatedUser).ThenInclude(u => u.Roles).ThenInclude(r => r.Claims);
            
        // Determine if the update histories should be fetched.
        bool shouldIncludeUpdateHistories = CanAccessUpdateHistories(_authorizationService);
        if (shouldIncludeUpdateHistories)
        {
            query = query.Include(d => d.UpdateHistories);
        }

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
    /// Generates a list of the <see cref="MonthYearResponseDto"/> instances, representing the
    /// options that users can select as filtering condition when fetching a list of
    /// <see cref="T"/> DTOs.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of <see cref="MonthYearResponseDto"/> instances, representing
    /// the options.
    /// </returns>
    public async Task<List<MonthYearResponseDto>> GenerateMonthYearOptions()
    {
        EarliestRecordedMonthYear ??= await GetRepository(_context)
            .OrderBy(T.StatsDateTimeExpression)
            .Select(entity => new MonthYearResponseDto
            {
                Year = entity.StatsDateTime.Year,
                Month = entity.StatsDateTime.Month
            }).FirstOrDefaultAsync();

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        int currentYear = currentDateTime.Year;
        int currentMonth = currentDateTime.Month;
        List<MonthYearResponseDto> monthYearOptions = new List<MonthYearResponseDto>();
        if (EarliestRecordedMonthYear != null)
        {
            for (int initializingYear = EarliestRecordedMonthYear.Year;
                initializingYear <= currentYear;
                initializingYear++)
            {
                int initializingMonth = 1;
                if (initializingYear == EarliestRecordedMonthYear.Year)
                {
                    initializingMonth = EarliestRecordedMonthYear.Month;
                }
                
                while (initializingMonth <= 12)
                {
                    MonthYearResponseDto option;
                    option = new MonthYearResponseDto(
                        initializingYear,
                        initializingMonth);
                    monthYearOptions.Add(option);
                    initializingMonth++;
                    if (initializingYear == currentYear && initializingMonth > currentMonth)
                    {
                        break;
                    }
                }
            }
            monthYearOptions.Reverse();
        }
        else
        {
            monthYearOptions.Add(new MonthYearResponseDto(currentYear, currentMonth));
        }

        return monthYearOptions;
    }

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
    protected abstract bool CanAccessUpdateHistories(IAuthorizationInternalService service);

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
    protected abstract bool CanDelete(T entity, IAuthorizationInternalService service);

    /// <summary>
    /// Stores the month and year information of the entity instance which is created earliest
    /// in the table as static value and doesn't change over requests.
    /// </summary>
    protected static MonthYearResponseDto EarliestRecordedMonthYear { get; set; }
}