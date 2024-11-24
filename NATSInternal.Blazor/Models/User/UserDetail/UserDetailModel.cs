namespace NATSInternal.Blazor.Models;

public class UserDetailModel
{
    public int Id { get; private set; }
    public string UserName { get; private set; }
    public UserPersonalInformationModel PersonalInformation { get; private set; }
    public UserUserInformationModel UserInformation { get; private set; }
    public UserDetailAuthorizationModel Authorization { get; private set; }
    public string UpdateRoute => $"user/{Id}/update";
    public string PasswordChangeRoute => $"user/passwordChange";
    public string PasswordResetRoute => $"user/{Id}/passwordReset";

    public UserDetailModel(UserDetailResponseDto responseDto)
    {
        Id = responseDto.Id;
        UserName = responseDto.UserName;
        PersonalInformation = new UserPersonalInformationModel(
            responseDto.PersonalInformation);
        UserInformation = new UserUserInformationModel(responseDto.UserInformation);
        Authorization = new UserDetailAuthorizationModel(responseDto.Authorization);
    }
}