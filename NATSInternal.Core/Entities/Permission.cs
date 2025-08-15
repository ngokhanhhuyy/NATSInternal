namespace NATSInternal.Core.Entities;

internal class Permission : IHasIdEntity<Permission>
{
    #region Fields
    private Role? _role;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Required]
    [StringLength(PermissionContracts.NameMaxLength)]
    public required string Name { get; set; }
    #endregion

    #region ForeignKeyProperties
    public required Guid RoleId { get; set; }
    #endregion

    #region NavigationProperties
    // Navigation properties.
    public Role Role
    {
        get => _role ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(Role)));
        set => _role = value;
    }
    #endregion

    #region Methods
    public static void ConfigureModel(EntityTypeBuilder<Permission> entityBuilder)
    {
        entityBuilder.HasKey(p => p.Id);
        entityBuilder.HasOne(p => p.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleId)
            .IsRequired();
    }
    #endregion
}