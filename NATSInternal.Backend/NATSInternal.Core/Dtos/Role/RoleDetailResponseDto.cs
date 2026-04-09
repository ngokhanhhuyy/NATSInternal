namespace NATSInternal.Core.Dtos;

public class RoleDetailResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public int PowerLevel { get; set; }
    public List<string> PermissionNames { get; set; }
    #endregion

    #region Constructors
    internal RoleDetailResponseDto(Role role)
    {
        Id = role.Id;
        Name = role.Name;
        DisplayName = role.DisplayName;
        PowerLevel = role.PowerLevel;
        PermissionNames = role.Permissions.Select(p => p.Name).ToList();
    }
    #endregion
}
