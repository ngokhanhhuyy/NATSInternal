namespace NATSInternal.Services.Dtos;

public class UserUserInformationRequestDto
    : IRequestDto
{
    public DateOnly? JoiningDate { get; set; }
    public string Note { get; set; }
    public RoleRequestDto Role { get; set; }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
    }
}
