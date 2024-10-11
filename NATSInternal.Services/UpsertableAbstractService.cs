namespace NATSInternal.Services;

internal abstract class UpsertableAbstractService<
        T,
        TListRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    where T : class, IIdentifiableEntity<T>, new()
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
    private readonly IAuthorizationInternalService _authorizationService;

    protected UpsertableAbstractService(IAuthorizationInternalService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    protected virtual async Task<TListResponseDto> GetListAsync(
        IQueryable<T> query,
        TListRequestDto requestDto)
    {
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
    /// <param name="query">s
    /// The query instance used to fetch the entity.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO
    /// containing the details of the debt payment.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the entity with the specified <c>id</c> arguments doesn't exist or has
    /// already been deleted.
    /// </exception>
    protected virtual async Task<TDetailResponseDto> GetDetailAsync(IQueryable<T> query)
    {
        return InitializeDetailResponseDto(await query.SingleOrDefaultAsync());
    }

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
}