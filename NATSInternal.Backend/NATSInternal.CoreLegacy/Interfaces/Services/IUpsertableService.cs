namespace NATSInternal.Core.Interfaces.Services;

public interface IUpsertableService<TEntity, TExistingAuthorizationResponseDto>
    where TEntity : class
    where TExistingAuthorizationResponseDto : IUpsertableExistingAuthorizationResponseDto
{
    #region Methods
    /// <summary>
    /// Check if the requesting user has permission to create a new <typeparamref name="TEntity"/>
    /// entity.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();

    /// <summary>
    /// Retrieve the authorization information for an existing <typeparamref name="TEntity"/> entity.
    /// </summary>
    /// <param name="entity">
    /// The instance of the <typeparamref name="TEntity"/> entity to retrieve the authorization information.
    /// </param>
    /// <returns>
    /// A DTO containing the authorization information.
    /// </returns>
    TExistingAuthorizationResponseDto GetExistingAuthorization(TEntity entity);
    #endregion
}
