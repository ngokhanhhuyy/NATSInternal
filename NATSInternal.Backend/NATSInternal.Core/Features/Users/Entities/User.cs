using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Users;

internal class User
{
    #region Properties
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(UserContracts.UserNameMaxLength)]
    public required string UserName { get; set; }
    
    [Required]
    [StringLength(UserContracts.PasswordHashMaxLength)]
    public required string PasswordHash { get; set; }
    #endregion
    
    #region ForeignKeyProperties
    [Required]
    public int CreatedUserId { get; set; }
    
    [Required]
    public int? LastUpdatedUserId { get; set; }
    
    [Required]
    public int? DeletedUserId { get; set; }
    
    [Required]
    public bool IsDeleted
    #endregion
    
    #region NavigationProperties
    public List<Role> Roles { get; set; } = new();
    #endregion
}