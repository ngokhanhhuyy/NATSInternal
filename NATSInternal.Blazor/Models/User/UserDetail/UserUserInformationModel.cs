namespace NATSInternal.Blazor.Models;

public class UserUserInformationModel
{
    public DateOnly? JoiningDate { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public string Note { get; set; }
    public RoleMinimalModel Role { get; set; }

    public UserUserInformationModel(UserUserInformationResponseDto responseDto)
    {
        JoiningDate = responseDto.JoiningDate;
        CreatedDateTime = responseDto.CreatedDateTime;
        UpdatedDateTime = responseDto.UpdatedDateTime;
        Note = responseDto.Note;
        Role = new RoleMinimalModel(responseDto.Role);
    }
}