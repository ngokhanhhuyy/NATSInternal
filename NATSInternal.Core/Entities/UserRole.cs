namespace NATSInternal.Core.Entities;

internal class UserRole : IdentityUserRole<int>, IEntity<UserRole>
{
    // Navigation properties
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<UserRole> entityBuilder)
    {
        entityBuilder.HasKey(ur => new { ur.UserId, ur.RoleId });
    }
}