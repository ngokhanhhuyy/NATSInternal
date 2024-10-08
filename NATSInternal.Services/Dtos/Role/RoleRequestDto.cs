namespace NATSInternal.Services.Dtos;

public class RoleRequestDto : IRequestDto
{
    public string Name { get; set; }

    public void TransformValues()
    {
        Name = Name?.ToNullIfEmpty();
    }
}