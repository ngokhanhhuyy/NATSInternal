using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Users;

internal class Permission
{
    #region Properties
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(PermissionContracts.NameMaxLength)]
    public required string Name { get; set; }
    #endregion
    
    #region ForeignKeyProperties
    [Required]
    public int RoleId { get; set; }
    #endregion
    
    #region NavigationProperties
    public Role Role { get; set; } = null!;
    #endregion
}
