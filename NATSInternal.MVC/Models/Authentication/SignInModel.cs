namespace NATSInternal.Models;

public class SignInModel
{
    [Display(Name = DisplayNames.UserName)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Password)]
    public string Password { get; set; }

    public SignInRequestDto ToRequestDto()
    {
        return new SignInRequestDto
        {
            UserName = UserName,
            Password = Password
        };
    }
}
