namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(NotificationEntityConfiguration))]
[Table("notifications")]
internal class Notification : AbstractEntity<Notification>, IUpsertableEntity<Notification>
{
    #region Fields
    private User? _createdUser;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("type")]
    [Required]
    public NotificationType Type { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; protected set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("resource_ids")]
    [StringLength(5000)]
    public List<Guid> ResourceIds { get; set; } = new();
    #endregion

    #region ForeignKeyProperties
    [Column("created_user_id")]
    public Guid? CreatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_createdUser))]
    public User? CreatedUser
    {
        get => _createdUser;
        set
        {
            CreatedUserId = value?.Id;
            _createdUser = value;
        }
    }

    public List<User> ReceivedUsers { get; protected set; } = new();
    public List<User> ReadUsers { get; protected set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public Guid ResourcePrimaryId => ResourceIds[0];

    [NotMapped]
    public Guid ResourceSecondaryId => ResourceIds[1];
    #endregion
}
