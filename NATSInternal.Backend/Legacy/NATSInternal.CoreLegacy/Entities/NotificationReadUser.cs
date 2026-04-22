namespace NATSInternal.Core.Entities;

[Table("notification_read_users")]
internal class NotificationReadUser : AbstractEntity<NotificationReadUser>
{
    #region Fields
    private Notification? _readNotification;
    private User? _readUser;
    #endregion

    #region ForeignKeyProperties
    [Column("read_notification_id")]
    [Key]
    public Guid ReadNotificationId { get; set; }

    [Column("read_user_id")]
    [Key]
    public Guid ReadUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_readNotification))]
    public Notification ReadNotification
    {
        get => GetFieldOrThrowIfNull(_readNotification);
        set
        {
            ReadNotificationId = value.Id;
            _readNotification = value;
        }
    }
    
    [BackingField(nameof(_readUser))]
    public User ReadUser
    {
        get => GetFieldOrThrowIfNull(_readUser);
        set
        {
            ReadUserId = value.Id;
            _readUser = value;
        }
    }
    #endregion
}
