namespace NATSInternal.Blazor.Models;

public class UserDetailModel
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public UserPersonalInformationModel PersonalInformation { get; set; }
    public UserUserInformationModel UserInformation { get; set; }

    public UserDetailModel(UserDetailResponseDto responseDto)
    {
        Id = responseDto.Id;
        UserName = responseDto.UserName;
        PersonalInformation = new UserPersonalInformationModel(
            responseDto.PersonalInformation);
        UserInformation = new UserUserInformationModel(responseDto.UserInformation);
    }
}