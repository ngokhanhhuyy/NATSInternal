namespace NATSInternal.Core.Dtos;

public class RoleMinimalResponseDto : IMinimalResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    #endregion

    #region Constructors
    internal RoleMinimalResponseDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
        DisplayName = role.DisplayName;
    }
    #endregion
}
