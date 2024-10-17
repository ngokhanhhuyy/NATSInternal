namespace NATSInternal.Services;

/// <summary>
/// An abstract service to handle financial engagement related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity associated to the <see cref="T"/> entity.
/// </typeparam>
/// <typeparam name="TListRequestDto">
/// The type of the request DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TUpdateHistoryDataDto">
/// The type of the update history data DTO, containing the data of a specific <see cref="T"/>
/// entity instance after each modification, used in the updating operation.
/// </typeparam>
internal abstract class FinancialEngageableAbstractService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TUpdateHistoryDataDto>
    : UpsertableAbstractService<T, TListRequestDto>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto : IFinancialEngageableListRequestDto
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;

    protected FinancialEngageableAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Gets a list of entities and month-year options, based on the specified query and
    /// paginating conditions.
    /// </summary>
    /// <param name="query">
    /// An instance of query which can be translated into SQL.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> which representing the asynchronous operation, which result is
    /// a DTO, containing a list of entities and the additional information for pagination.
    /// </returns>
    protected override async Task<EntityListDto<T>>
            GetListOfEntitiesAsync(IQueryable<T> query, TListRequestDto requestDto)
    {
        IQueryable<T> monthYearFilteredQuery = query;

        // Filter by month and year if specified.
        if (!requestDto.IgnoreMonthYear)
        {
            DateTime startDateTime = new DateTime(requestDto.Year, requestDto.Month, 1);
            DateTime endDateTime = startDateTime.AddMonths(1);
            monthYearFilteredQuery = monthYearFilteredQuery.Where(di =>
                di.StatsDateTime >= startDateTime && di.StatsDateTime < endDateTime);
        }

        // Filter by created user id if specified.
        if (requestDto.CreatedUserId.HasValue)
        {
            monthYearFilteredQuery = monthYearFilteredQuery
                .Where(e => e.CreatedUserId == requestDto.CreatedUserId);
        }

        EntityListDto<T> entityListDto;
        entityListDto = await base.GetListOfEntitiesAsync(monthYearFilteredQuery, requestDto);
        
        return entityListDto;
    }

    /// <summary>
    /// Retrieves an entity based on the specified id.
    /// </summary>
    /// <remarks>
    /// The related update history entities will be included if the requesting user has enough
    /// permission to access them.
    /// </remarks>
    /// <param name="query">
    /// An instance of the query that can be translated into SQL query.
    /// </param>
    /// <param name="id">
    /// The id of the entity to retrieve.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="T"/> entity with the specified id.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the entity with the specified id doesn't exist or has already been deleted.
    /// </exception>
    protected virtual async Task<T> GetEntityAsync(IQueryable<T> query, int id)
    {
        IQueryable<T> includedQuery = query;
        if (CanAccessUpdateHistories(_authorizationService))
        {
            includedQuery = includedQuery.Include(e => e.UpdateHistories);
        }

        return await includedQuery.SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted)
            ?? throw new ResourceNotFoundException(
                typeof(T).Name,
                GetPropertyName<T>(e => e.Id),
                id.ToString());
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
    protected void LogUpdateHistory(
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
    /// Validates if the specified <c>statsDateTime</c> argument is valid for an entity so that
    /// its locking status won't change after the assignment.
    /// </summary>
    /// <param name="entity">
    /// An instance of the entity class to which the <c>statsDateTime</c> argument is assigned.
    /// </param>
    /// <param name="statsDateTime">
    /// A <see cref="DateTime"/> value specified in the request representing the date and time
    /// for the field in the entity which is used to calculate the statistics.
    /// </param>
    /// <exception cref="ValidationException">
    /// Throws when the value specified by the <c>statsDateTime</c> argument is invalid.
    /// </exception>
    protected static void ValidateStatsDateTime(T entity, DateTime statsDateTime)
    {
        string errorMessage;
        if (statsDateTime > entity.CreatedDateTime)
        {
            errorMessage = ErrorMessages.EarlierThanOrEqual
                .ReplaceComparisonValue(entity.CreatedDateTime.ToVietnameseString());
            throw new ArgumentException(errorMessage);
        }

        DateTime minimumAssignableDateTime;
        minimumAssignableDateTime = new DateTime(
            entity.CreatedDateTime.AddMonths(-1).Year,
            entity.CreatedDateTime.AddMonths(-1).Month,
            1, 0, 0, 0);
        if (statsDateTime < minimumAssignableDateTime)
        {
            errorMessage = ErrorMessages.GreaterThanOrEqual
                .ReplaceComparisonValue(minimumAssignableDateTime.ToVietnameseString());
            throw new ValidationException(errorMessage);
        }
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
    protected async Task<List<MonthYearResponseDto>> GenerateMonthYearOptions()
    {
        EarliestRecordedMonthYear ??= await GetRepository(_context)
            .OrderBy(e => e.StatsDateTime)
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
    /// Gets the entity repository in the <see cref="DatabaseContext"/> class.
    /// </summary>
    /// <param name="context">
    /// An instance of the injected <see cref="DatabaseContext"/>
    /// </param>
    /// <returns>The entity repository.</returns>
    protected abstract DbSet<T> GetRepository(DatabaseContext context);

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
    /// Stores the month and year information of the entity instance which is created earliest
    /// in the table as static value and doesn't change over requests.
    /// </summary>
    protected static MonthYearResponseDto EarliestRecordedMonthYear { get; set; }
}