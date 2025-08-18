namespace NATSInternal.Core.Entities;

[Table("permissions")]
internal class Permission : AbstractHasIdEntity<Permission>
{
    #region Fields
    private Role? _role;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public override Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("name")]
    [Required]
    [StringLength(PermissionContracts.NameMaxLength)]
    public required string Name { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("role_id")]
    [Required]
    public required Guid RoleId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_role))]
    public Role Role
    {
        get => GetFieldOrThrowIfNull(_role);
        set
        {
            RoleId = value.Id;
            _role = value;
        }
    }
    #endregion
}