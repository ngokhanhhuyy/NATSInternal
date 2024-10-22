namespace NATSInternal.Blazor.Models;

public class UserListAuthorizationModel
{
    public bool CanCreate { get; set; }

    public UserListAuthorizationModel(UserListAuthorizationResponseDto responseDto)
    {
        CanCreate = responseDto.CanCreate;
    }
}