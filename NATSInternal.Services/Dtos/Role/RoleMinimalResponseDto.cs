namespace NATSInternal.Services.Dtos;

public class RoleMinimalResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    internal RoleMinimalResponseDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
    }
}
