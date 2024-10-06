namespace NATSInternal.Services.Entities;

internal class Role : IdentityRole<int>, IIdentifiableEntity<Role>
{
    [Required]
    [StringLength(50)]
    public string DisplayName { get; set; }

    [Required]
    public int PowerLevel { get; set; }

    // Navigation properties.
    public virtual List<User> Users { get; set; }
    public virtual List<IdentityRoleClaim<int>> Claims { get; set; }
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<Role> entityBuilder)
    {
        entityBuilder.HasKey(r => r.Id);
        entityBuilder.HasIndex(r => r.Name)
            .IsUnique();
        entityBuilder.HasIndex(r => r.DisplayName)
            .IsUnique();
    }
}