namespace NATSInternal.Services;

internal abstract class UpsertableAbstractService<
        T,
        TListRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    where T : class, IUpsertableEntity<T>, new()
    where TListRequestDto : IOrderableListRequestDto
    where TListResponseDto :
        IUpsertableListResponseDto<
            TBasicResponseDto,
            TAuthorizationResponseDto,
            TListAuthorizationResponseDto>,
        new()
    where TBasicResponseDto : class, IUpsertableBasicResponseDto<TAuthorizationResponseDto>
    where TDetailResponseDto : IBasicResponseDto
    where TListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
    where TAuthorizationResponseDto : IUpsertableAuthorizationResponseDto
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;

    protected UpsertableAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Gets a list of entities, based on the specified filtering, sorting and paginating
    /// conditions.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> which representing the asynchronous operation, which result is
    /// a DTO, containing the results and the additional information for pagination.
    /// </returns>
    public virtual async Task<TListResponseDto> GetListAsync(TListRequestDto requestDto)
    {
        IQueryable<T> query = InitializeListQuery(requestDto);

        // Initialize response dto.
        TListResponseDto responseDto = new TListResponseDto();

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
        responseDto.Authorization = InitializeListAuthorizationResponseDto(
            _authorizationService);

        return responseDto;
    }

    /// <summary>
    /// Retrieves the details of a specific entity by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the entity to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO
    /// containing the details of the debt payment.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the entity with the specified <c>id</c> arguments doesn't exist or has
    /// already been deleted.
    /// </exception>
    public virtual async Task<TDetailResponseDto> GetDetailAsync(int id)
    {
        IQueryable<T> query = InitializeDetailQuery().Where(e => e.Id == id);
        T entity = await query.SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                typeof(T).Name,
                nameof(id),
                id.ToString());

        return InitializeDetailResponseDto(entity);
    }

    /// <summary>
    /// Gets the entity repository in the <see cref="DatabaseContext"/> class.
    /// <param name="context">
    /// An instance of the injected <see cref="DatabaseContext"/>
    /// </param>
    /// <returns>The entity repository.</returns>
    protected abstract DbSet<T> GetRepository(DatabaseContext context);

    /// <summary>
    /// Initializes the query for list retrieving operation, based on the sorting conditions
    /// specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A query instance used to perform the list retrieving operation.
    /// </returns>
    protected virtual IQueryable<T> InitializeListQuery(TListRequestDto requestDto)
    {
        IQueryable<T> query = GetRepository(_context);

        // Sort by the specified direction and field.
        return SortListQuery(query, requestDto.OrderByAscending, requestDto.OrderByField);
    }

    /// <summary>
    /// Initializes the query for detail retrieving operation, based on specified id of the
    /// entity.
    /// </summary>
    /// <returns>
    /// A query instance used to perform the detail retrieving operation.
    /// </returns>
    protected virtual IQueryable<T> InitializeDetailQuery()
    {
        return GetRepository(_context);
    }

    /// <summary>
    /// Provides the sorting conditions for the list retrieving operation, based on the
    /// specified conditions.
    /// </summary>
    /// <param name="query">
    /// An initialized query instance.
    /// </param>
    /// <param name="orderByAscending">
    /// Indicates whether the results should be ordered by ascending.
    /// </param>
    /// <param name="orderByField">
    /// Indicates the name of the field by which the results should be ordered.
    /// </param>
    /// <returns>
    /// A sorted query used for list retrieving operation.
    /// </returns>
    protected abstract IOrderedQueryable<T> SortListQuery(
            IQueryable<T> query,
            bool orderByAscending,
            string orderByField);

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
    /// <returns>The initialized DTO.</returns>
    protected abstract TDetailResponseDto InitializeDetailResponseDto(T entity);

    /// <summary>
    /// Initializes a response DTO, containing the authorization information for the list
    /// response DTO, used in the list retrieving operation.
    /// </summary>
    /// <param name="service">
    /// The authorization service to retrieve the authorization information.
    /// </param>
    /// <returns>The initialized DTO.</returns>
    protected abstract TListAuthorizationResponseDto InitializeListAuthorizationResponseDto(
            IAuthorizationInternalService service);

    /// <summary>
    /// Initializes a response DTO, containing the authorization information of a specified
    /// entity for the basic and the detail response DTO, used in the list retrieving and
    /// detail retriving operation.
    /// </summary>
    /// <param name="entity">
    /// The entity to retrive the authorization for.
    /// </param>
    /// <param name="service">
    /// The authorization service to retrieve the authorization information.
    /// </param>
    /// <returns>The initialized DTO.</returns>
    protected abstract TAuthorizationResponseDto InitializeAuthorizationResponseDto(
            T entity,
            IAuthorizationInternalService service);
}