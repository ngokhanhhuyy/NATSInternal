namespace NATSInternal.Core.Dtos;

public class RoleMinimalResponseDto : IMinimalResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }

    internal RoleMinimalResponseDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
        DisplayName = role.DisplayName;
    }
}
