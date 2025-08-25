namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle authentication-related operations.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Verify if the specified username exists and its corresponding password is correct.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="SignInRequestDto"/> class, containing the username and the password for the sign
    /// in operation.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A cancellation token.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="UserDetailResponseDto"/> containing the detailed information of the user who has
    /// the userName specified in the <paramref name="requestDto"/>.
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the user with the specified username doesn't exist or has already been deleted.
    /// - When the specified password is incorrect.
    /// </exception>
    Task<UserDetailResponseDto> VerifyUserNameAndPasswordAsync(
            SignInRequestDto requestDto,
            CancellationToken cancellationToken = default);
}
