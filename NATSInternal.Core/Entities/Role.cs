namespace NATSInternal.Core.Entities;

internal class Role : IHasIdEntity<Role>
{
    #region Properties
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Required]
    [StringLength(RoleContracts.NameMaxLength)]
    public required string Name { get; set; }

    [Required]
    [StringLength(RoleContracts.DisplayNameMaxLength)]
    public string DisplayName { get; set; } = string.Empty;

    [Required]
    public int PowerLevel { get; set; }
    #endregion

    #region NavigationProperties.
    public List<User> Users { get; private set; } = new();
    public List<Permission> Permissions { get; private set; } = new();
    #endregion
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<Role> entityBuilder)
    {
        entityBuilder.HasKey(r => r.Id);
        entityBuilder.HasIndex(r => r.Name).IsUnique();
        entityBuilder.HasIndex(r => r.DisplayName).IsUnique();
    }
}