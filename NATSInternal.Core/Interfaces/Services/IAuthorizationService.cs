namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle the operations which are related to authorization.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Sets the id of the requesting user for later operations.
    /// </summary>
    /// <param name="id">
    /// A <see cref="Guid"/> value, which is extracted from the request's credentials, representing the id of the
    /// requesting user.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task SetUserId(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the id of the requesting user.
    /// </summary>
    /// <returns>
    /// The id of the requesting user.
    /// </returns>
    Guid GetUserId();
    
    /// <summary>
    /// Retrieves the details of the requesting user.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="UserDetailResponseDto"/> class, containing the details of the user.
    /// </returns>
    UserDetailResponseDto GetUserDetail();
}
