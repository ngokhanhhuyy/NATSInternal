using NATSInternal.Application.UseCases.Authentication;

namespace NATSInternal.Api.Models;

public class SignInModel
{
    #region Properties
    public required string UserName { get; set; }
    public required string Password { get; set; }
    #endregion

    #region Methods
    public VerifyUserNameAndPasswordRequestDto ToRequestDto()
    {
        return new()
        {
            UserName = UserName,
            Password = Password
        };
    }
    #endregion
}