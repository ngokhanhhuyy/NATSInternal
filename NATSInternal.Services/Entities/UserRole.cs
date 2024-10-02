namespace NATSInternal.Services.Entities;

[Table("user_roles")]
internal class UserRole : IdentityUserRole<int>
{
    // Navigation properties
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}