namespace NATSInternal.Core.Entities;

[Table("roles")]
internal class Role : IHasIdEntity<Role>
{
    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Column("name")]
    [Required]
    [StringLength(RoleContracts.NameMaxLength)]
    public required string Name { get; set; }

    [Column("display_name")]
    [Required]
    [StringLength(RoleContracts.DisplayNameMaxLength)]
    public string DisplayName { get; set; } = string.Empty;

    [Column("power_level")]
    [Required]
    public int PowerLevel { get; set; }
    #endregion

    #region NavigationProperties.
    public List<User> Users { get; protected set; } = new();
    public List<Permission> Permissions { get; protected set; } = new();
    #endregion
}