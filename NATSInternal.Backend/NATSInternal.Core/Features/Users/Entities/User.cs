using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    
    [Required]
    public required DateTime CreatedDateTime { get; set; }
    public DateTime? LastUpdatedDateTime { get; set; }
    public DateTime? DeletedDateTime { get; set; }
    #endregion
    
    #region ForeignKeyProperties
    public required int? CreatedUserId { get; set; }
    public int? LastUpdatedUserId { get; set; }
    public int? DeletedUserId { get; set; }
    #endregion
    
    #region NavigationProperties
    public User? CreatedUser { get; set; }
    public User? LastUpdatedUser { get; set; }
    public User? DeletedUser { get; set; }
    public List<Role> Roles { get; set; } = new();
    #endregion
    
    #region ComputedProperties
    [NotMapped]
    public int MaxRolePowerLevel => Roles.Max(r => r.PowerLevel);
    #endregion
}