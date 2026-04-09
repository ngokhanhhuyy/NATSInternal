namespace NATSInternal.Core.Entities;

internal class UserRole : AbstractEntity<UserRole>
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
        get => GetFieldOrThrowIfNull(_user);
        set
        {
            UserId = value.Id;
            _user = value;
        }
    }

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