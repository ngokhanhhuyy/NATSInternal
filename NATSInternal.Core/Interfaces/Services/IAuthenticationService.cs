namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle authentication-related operations.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Signs in with the specified username and password using cookies.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="SignInRequestDto"/> class, containing the username and the password for the sign
    /// in operation.
    /// </param>
    /// <returns>
    /// The id of the signed in user.
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the user with the specified username doesn't exist or has already been deleted.
    /// - When the specified password is incorrect.
    /// </exception>
    Task<Guid> SignInAsync(SignInRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Signs out and clear the cookies which contains the authentication credentials from the requesting user.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task SignOutAsync(CancellationToken cancellationToken = default);
}
