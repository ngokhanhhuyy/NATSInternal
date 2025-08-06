namespace NATSInternal.Core.Dtos;

public class UserUserInformationResponseDto
{
    public DateOnly? JoiningDate { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public string Note { get; set; }
    public RoleMinimalResponseDto Role { get; set; }

    internal UserUserInformationResponseDto(User user)
    {
        JoiningDate = user.JoiningDate;
        CreatedDateTime = user.CreatedDateTime;
        UpdatedDateTime = user.UpdatedDateTime;
        Note = user.Note;
        Role = new RoleMinimalResponseDto(user.Role);
    }
}
