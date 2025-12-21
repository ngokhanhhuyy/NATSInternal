using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Authentication;

namespace NATSInternal.Api.Models;

public class SignInModel
{
    #region Properties
    [DisplayName(DisplayNames.UserName)]
    public string UserName { get; set; } = string.Empty;
    
    [DisplayName(DisplayNames.Password)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
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