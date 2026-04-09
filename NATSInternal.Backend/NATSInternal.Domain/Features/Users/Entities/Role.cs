using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

internal class Role : AbstractEntity
{
    #region Fields
    private readonly List<Permission> _permissions = new();
    #endregion

    #region Constructors
#nullable disable
    private Role() { }
#nullable enable

    public Role(string name, string displayName, int powerLevel, ICollection<string> permissionNames)
    {
        Name = name;
        DisplayName = displayName;
        PowerLevel = powerLevel;
        _permissions.AddRange(permissionNames.Select(n => new Permission(n, Id)));
    }
    #endregion
    
    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string DisplayName { get; private set; }
    public int PowerLevel { get; private set; }
    #endregion

    #region NavigationProperties.
    public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();
    #endregion
}