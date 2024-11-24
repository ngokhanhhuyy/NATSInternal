namespace NATSInternal.Blazor.Models;

public class UserDetailAuthorizationModel
{
    public bool CanGetNote { get; private set; }
    public bool CanEdit { get; private set; }
    public bool CanEditUserPersonalInformation { get; private set; }
    public bool CanEditUserUserInformation { get; private set; }
    public bool CanAssignRole { get; private set; }
    public bool CanChangePassword { get; private set; }
    public bool CanResetPassword { get; private set; }
    public bool CanDelete { get; set; }

    public UserDetailAuthorizationModel(UserDetailAuthorizationResponseDto responseDto)
    {
        CanGetNote = responseDto.CanGetNote;
        CanEdit = responseDto.CanEdit;
        CanEditUserPersonalInformation = responseDto.CanEditUserPersonalInformation;
        CanEditUserUserInformation = responseDto.CanEditUserUserInformation;
        CanAssignRole = responseDto.CanAssignRole;
        CanChangePassword = responseDto.CanChangePassword;
        CanResetPassword = responseDto.CanResetPassword;
        CanDelete = responseDto.CanDelete;
    }
}