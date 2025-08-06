namespace NATSInternal.Core.Dtos;

public class UserCreateRequestDto : IRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmationPassword { get; set; }
    public UserPersonalInformationRequestDto PersonalInformation { get; set; }
    public UserUserInformationRequestDto UserInformation { get; set; }

    public void TransformValues()
    {
        UserName = UserName?.ToNullIfEmpty();
        Password = Password?.ToNullIfEmpty();
        ConfirmationPassword = ConfirmationPassword?.ToNullIfEmpty();
        PersonalInformation.TransformValues();
        UserInformation.TransformValues();
    }
}