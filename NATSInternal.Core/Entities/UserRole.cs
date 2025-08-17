namespace NATSInternal.Core.Entities;

internal class UserRole : IEntity<UserRole>
{
    #region Fields
    private User? _user;
    private Role? _role;
    #endregion

    #region Properties
    [Key]
    [Required]
    public Guid UserId { get; set; }

    [Key]
    [Required]
    public Guid RoleId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_user))]
    public User User
    {
        get => _user ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(User)));
        set
        {
            UserId = value.Id;
            _user = value;
        }
    }

    [BackingField(nameof(_role))]
    public Role Role
    {
        get => _role ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(Role)));
        set
        {
            RoleId = value.Id;
            _role = value;
        }
    }
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<UserRole> entityBuilder)
    {
        entityBuilder.HasKey(ur => new { ur.UserId, ur.RoleId });
    }
    #endregion
}