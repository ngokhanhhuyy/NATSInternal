namespace NATSInternal.Services;

/// <summary>
/// An abstract service to handle financial engagement related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity associated to the <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TListRequestDto">
/// The type of the request DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TUpdateHistoryDataDto">
/// The type of the update history data DTO, containing the data of a specific
/// <typeparamref name="T"/> entity instance after each modification, used in the updating
/// operation.
/// </typeparam>
/// <typeparam name="TCreatingAuthorizationResponseDto">
/// The type of response DTO which contains the authorization information when creating a new
/// <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TExistingAuthorizationResponseDto">
/// The type of response DTO which contains the authorization information when updating an
/// existing <typeparamref name="T"/> entity.
/// </typeparam>
internal abstract class HasStatsAbstractService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TUpdateHistoryDataDto,
        TCreatingAuthorizationResponseDto,
        TExistingAuthorizationResponseDto>
    : UpsertableAbstractService<T, TListRequestDto, TExistingAuthorizationResponseDto>
    where T : class, IHasStatsEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto : IHasStatsListRequestDto
    where TCreatingAuthorizationResponseDto :
        class,
        IHasStatsCreatingAuthorizationResponseDto,
        new()
    where TExistingAuthorizationResponseDto : IHasStatsExistingAuthorizationResponseDto, new()
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;

    protected HasStatsAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService) : base(authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Retrieve a list of the <see cref="ListMonthYearResponseDto"/> instances, representing
    /// the options that users can select as filtering condition in list retrieving operation.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// a <see cref="List{T}"/> of <see cref="ListMonthYearResponseDto"/> instances,
    /// representing the options.
    /// </returns>
    public async Task<ListMonthYearOptionsResponseDto> GetListMonthYearOptionsAsync()
    {
        EarliestRecordedMonthYear ??= await GetRepository(_context)
            .OrderBy(e => e.StatsDateTime)
            .Select(entity => new ListMonthYearResponseDto
            {
                Year = entity.StatsDateTime.Year,
                Month = entity.StatsDateTime.Month
            }).FirstOrDefaultAsync();

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        int currentYear = currentDateTime.Year;
        int currentMonth = currentDateTime.Month;
        List<ListMonthYearResponseDto> monthYearOptions = new List<ListMonthYearResponseDto>();
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
                    ListMonthYearResponseDto option;
                    option = new ListMonthYearResponseDto(
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
            monthYearOptions.Add(new ListMonthYearResponseDto(currentYear, currentMonth));
        }

        return new ListMonthYearOptionsResponseDto
        {
            Options = monthYearOptions
        };
    }

    /// <summary>
    /// Get the authorization information for creating operation.
    /// </summary>
    /// <returns>
    /// An instance of the <typeparamref name="TExistingAuthorizationResponseDto"/> DTO
    /// containing the authorization information when the requesting user has creating
    /// permission. Otherwise,
    /// <c>null</c>.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have permission to create.
    /// </exception>
    public virtual TCreatingAuthorizationResponseDto GetCreatingAuthorization()
    {
        if (!GetCreatingPermission())
        {
            return null;
        }

        return _authorizationService
            .GetCreatingAuthorization<T, TUpdateHistory, TCreatingAuthorizationResponseDto>();
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
    /// A <see cref="Task"/> which representing the asynchronous operation, which
    /// result is a DTO, containing a list of entities and the additional information for
    /// pagination.
    /// </returns>
    protected override async Task<EntityListDto<T>>
            GetListOfEntitiesAsync(IQueryable<T> query, TListRequestDto requestDto)
    {
        IQueryable<T> monthYearFilteredQuery = query;

        // Filter by month and year if specified.
        if (requestDto.MonthYear != null)
        {
            DateTime startDateTime = new DateTime(
                requestDto.MonthYear.Year,
                requestDto.MonthYear.Month, 1);
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
    /// An instance of the <typeparamref name="T"/> entity with the specified id.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the entity with the specified id doesn't exist or has already been deleted.
    /// </exception>
    protected virtual async Task<T> GetEntityAsync(IQueryable<T> query, int id)
    {
        IQueryable<T> includedQuery = query;
        if (CanAccessUpdateHistories())
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
    /// An instance of the <typeparamref name="T"/> entity class, representing the entity to be
    /// logged.
    /// </param>
    /// <param name="oldData">
    /// An instance of the <typeparamref name="TUpdateHistoryDataDto"/> class, containing the
    /// data of the entity before the modification.
    /// </param>
    /// <param name="newData">
    /// An instance of the <typeparamref name="TUpdateHistoryDataDto"/> class, containing the
    /// data of the entity after the modification.
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
    /// Validates if the specified <paramref name="statsDateTime"/> is valid for an entity so
    /// that its locking status won't change after the assignment.
    /// </summary>
    /// <param name="entity">
    /// An instance of the entity class to which the <paramref name="statsDateTime"/> is
    /// assigned.
    /// </param>
    /// <param name="statsDateTime">
    /// A <see cref="DateTime"/> value specified in the request representing the date and time
    /// for the field in the entity which is used to calculate the statistics.
    /// </param>
    /// <exception cref="ValidationException">
    /// Throws when the value specified by the <paramref name="statsDateTime"/> is invalid.
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
    /// Gets the entity repository in the <see cref="DatabaseContext"/> class.
    /// </summary>
    /// <param name="context">
    /// An instance of the injected <see cref="DatabaseContext"/>
    /// </param>
    /// <returns>The entity repository.</returns>
    protected abstract DbSet<T> GetRepository(DatabaseContext context);

    /// <inheritdoc />
    protected override TExistingAuthorizationResponseDto GetExistingAuthorization(T entity)
    {
        return _authorizationService.GetExistingAuthorization<
            T,
            TUpdateHistory,
            TExistingAuthorizationResponseDto>(entity);
    }

    /// <summary>
    /// Determines whether the requesting user has enough permissions edit a specified
    ///  <typeparamref name="T"/> entity.
    /// </summary>
    /// <param name="entity">
    /// The instance of the <typeparamref name="T"/> entity to check the permission.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing whether the requesting user has the permission.
    /// </returns>
    protected bool CanEdit(T entity)
    {
        return _authorizationService.CanEdit<T, TUpdateHistory>(entity);
    }

    /// <summary>
    /// Determines whether the requesting user has enough permissions delete a specified
    /// <typeparamref name="T"/> entity.
    /// </summary>
    /// <param name="entity">
    /// The instance of the <typeparamref name="T"/> entity to check the permission.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing whether the requesting user has the permission.
    /// </returns>
    protected bool CanDelete(T entity)
    {
        return _authorizationService.CanDelete<T, TUpdateHistory>(entity);
    }

    /// <summary>
    /// Determines whether the requesting user has enough permissions to set value for the
    /// <c>StatsDateTime</c> of a new <typeparamref name="T"/> entity in a creating operation.
    /// </summary>
    /// <returns>
    /// A <see cref="bool"/> value representing whether the requesting user has the permission.
    /// </returns>
    protected bool CanSetStatsDateTimeWhenCreating()
    {
        return _authorizationService.CanSetStatsDateTimeWhenCreating<T, TUpdateHistory>();
    }

    /// <summary>
    /// Determines whether the requesting user has enough permissions to set value for the
    /// <c>StatsDateTime</c> property of a specified <typeparamref name="T"/> entity in an
    /// updating operation.
    /// </summary>
    /// <param name="entity">
    /// (Optional) The instance of the <typeparamref name="T"/> entity to check the permission.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing whether the requesting user has the permission.
    /// </returns>
    protected bool CanSetStatsDateTimeWhenEditing(T entity)
    {
        return _authorizationService.CanSetStatsDateTimeWhenEditing<T, TUpdateHistory>(entity);
    }

    /// <summary>
    /// Determines whether the current user has enough permissions to access the update history
    /// of any entity, used in the detail retrieving operation.
    /// </summary>
    /// <returns>
    /// A <see cref="bool"/> value representing the permission.
    /// </returns>
    protected bool CanAccessUpdateHistories()
    {
        return _authorizationService.CanAccessUpdateHistory<T, TUpdateHistory>();
    }

    /// <summary>
    /// Stores the month and year information of the entity instance which is created earliest
    /// in the table as static value and doesn't change over requests.
    /// </summary>
    protected static ListMonthYearResponseDto EarliestRecordedMonthYear { get; set; }
}