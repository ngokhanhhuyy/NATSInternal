namespace NATSInternal.Services.Dtos;

public class UserUpdateRequestDto : IRequestDto
{
    public UserPersonalInformationRequestDto PersonalInformation { get; set; }
    public UserUserInformationRequestDto UserInformation { get; set; }

    public void TransformValues()
    {
    }
}
