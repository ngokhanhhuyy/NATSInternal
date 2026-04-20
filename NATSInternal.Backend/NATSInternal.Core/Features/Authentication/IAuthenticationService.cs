namespace NATSInternal.Core.Features.Authentication;

public interface IAuthenticationService
{
    #region Methods
    Task VerifyUserNameAndPasswordAsync(VerifyUserNameAndPasswordRequestDto requestDto);
    Task ChangePasswordAsync(ChangePasswordRequestDto requestDto);
    Task ResetPasswordAsync(int id);
    #endregion
}