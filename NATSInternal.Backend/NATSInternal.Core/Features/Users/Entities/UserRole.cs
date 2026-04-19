using Microsoft.EntityFrameworkCore;

namespace NATSInternal.Core.Features.Users;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
internal class UserRole
{
    #region Properties
    public int UserId { get; set; }
    public int RoleId { get; set; }
    #endregion
    
    #region NavigationProperties
    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
    #endregion
}