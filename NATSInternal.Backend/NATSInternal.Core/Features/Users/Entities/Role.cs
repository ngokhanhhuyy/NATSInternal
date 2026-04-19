using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Users;

internal class Role
{
    #region Properties
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(RoleContracts.NameMaxLength)]
    public required string Name { get; set; }
    
    [Required]
    [StringLength(RoleContracts.DisplayNameMaxLength)]
    public required string DisplayName { get; set; }
    
    [Required]
    public required int PowerLevel { get; set; }
    #endregion
    
    #region NavigationProperties
    public List<User> Users { get; set; } = new();
    #endregion
}