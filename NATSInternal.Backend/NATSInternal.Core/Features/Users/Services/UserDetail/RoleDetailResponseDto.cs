namespace NATSInternal.Core.Features.Users;

public class RoleDetailResponseDto
{
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
    
    #region Properties
    public int Id { get; }
    public string Name { get; }
    public string DisplayName { get; }
    public int PowerLevel { get; }
    public List<string> PermissionNames { get; }
    #endregion
}