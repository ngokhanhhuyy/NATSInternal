using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Users;

internal class Permission : AbstractEntity
{
    #region Constructors
#nullable disable
    private Permission() { }
#nullable restore

    public Permission(string name, Guid roleId)
    {
        Name = name;
        RoleId = roleId;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    #endregion

    #region ForeignKeyProperties
    public Guid RoleId { get; private set; }
    #endregion
}