namespace NATSInternal.Core;

/// <summary>
/// An abstract service to handle upserting related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity.
/// </typeparam>
/// <typeparam name="TListRequestDto">
/// The type of the request DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TExistingAuthorizationResponseDto">
/// The type of response DTO which contains the authorization information for an existing
/// <typeparamref name="T"/> entity.
/// </typeparam>
internal abstract class UpsertableAbstractService<
            T,
            TListRequestDto,
            TExistingAuthorizationResponseDto>
        where T : class, IUpsertableEntity<T>, new()
        where TListRequestDto : ISortableListRequestDto
        where TExistingAuthorizationResponseDto : IUpsertableExistingAuthorizationResponseDto, new()
{
    private readonly IAuthorizationInternalService _authorizationService;

    protected UpsertableAbstractService(IAuthorizationInternalService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Get all fields those are used as options to order the results in list retrieving
    /// operation.
    /// </summary>
    /// <returns>
    /// A <see cref="List{T}"/> of the <see cref="ListSortingByFieldResponseDto"/> DTOs,
    /// containing the name and the display name of the field
    /// </returns>
    public abstract ListSortingOptionsResponseDto GetListSortingOptions();

    /// <summary>
    /// Check if the requesting user has permission to create a new <typeparamref name="T"/>
    /// entity.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    public bool GetCreatingPermission()
    {
        return _authorizationService.CanCreate<T>();
    }

    /// <summary>
    /// Gets a list of entities, based on the specified query and paginating conditions.
    /// </summary>
    /// <param name="query">
    /// An instance of query which can be translated into SQL.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> which representing the asynchronous operation, which result is a
    /// DTO, containing a list of entities and the additional information for pagination.
    /// </returns>
    protected virtual async Task<EntityListDto<T>> GetListOfEntitiesAsync(
            IQueryable<T> query,
            TListRequestDto requestDto)
    {
        // Initialize response dto.
        EntityListDto<T> entitiesDto = new EntityListDto<T>();

        // Fetch the result count.
        int resultCount = await query.CountAsync();
        if (resultCount == 0)
        {
            entitiesDto.PageCount = 0;
            return entitiesDto;
        }

        entitiesDto.PageCount = (int)Math.Ceiling(
            (double)resultCount / requestDto.ResultsPerPage);
        entitiesDto.Items = await query
            .Skip(requestDto.ResultsPerPage * (requestDto.Page - 1))
            .Take(requestDto.ResultsPerPage)
            .AsSplitQuery()
            .ToListAsync();

        return entitiesDto;
    }

    /// <summary>
    /// Get the name of a specific property from an expressing in string.
    /// </summary>
    /// <typeparam name="TParent">
    /// The type to which the property belongs.
    /// </typeparam>
    /// <param name="propertySelector">
    /// A lambda expression to select the property from the entity.
    /// </param>
    /// <returns>
    /// The name of the selected property in string.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Throws when the lambda expression cannot be read or is invalid.
    /// </exception>
    protected static string GetPropertyName<TParent>(
            Expression<Func<TParent, object>> propertySelector)
    {
        if (propertySelector.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        // For cases where the property is cast to object (e.g., value types)
        if (propertySelector.Body is UnaryExpression unaryExpression &&
            unaryExpression.Operand is MemberExpression operandExpression)
        {
            return operandExpression.Member.Name;
        }

        throw new ArgumentException("Invalid property expression.");
    }

    /// <summary>
    /// Retrieve the authorization information for an existing <typeparamref name="T"/> entity.
    /// </summary>
    /// <param name="entity">
    /// The instance of the <typeparamref name="T"/> entity to retrieve the authorization
    /// information.
    /// </param>
    /// <returns>
    /// A DTO containing the authorization information.
    /// </returns>
    protected virtual TExistingAuthorizationResponseDto GetExistingAuthorization(T entity)
    {
        return _authorizationService
            .GetExistingAuthorization<T, TExistingAuthorizationResponseDto>();
    }
}