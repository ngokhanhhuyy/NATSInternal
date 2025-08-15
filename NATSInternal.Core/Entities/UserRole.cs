namespace NATSInternal.Core.Entities;

internal class UserRole : IEntity<UserRole>
{
    // Property.
    [Key]
    [Required]
    public required Guid UserId { get; set; }
    
    [Key]
    [Required]
    public required Guid RoleId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<UserRole> entityBuilder)
    {
        entityBuilder.HasKey(ur => new { ur.UserId, ur.RoleId });
    }
}