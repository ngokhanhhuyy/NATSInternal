namespace NATSInternal.Core.Dtos;

public class UserUserInformationRequestDto
    : IRequestDto
{
    public DateOnly? JoiningDate { get; set; }
    public string Note { get; set; }
    public string RoleName { get; set; }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
        RoleName = RoleName?.ToNullIfEmpty();
    }
}
