namespace NATSInternal.Core.Entities;

[Table("notification_received_users")]
internal class NotificationReceivedUser : AbstractEntity<NotificationReadUser>, IEntity<NotificationReceivedUser>
{
    #region Fields
    private Notification? _receivedNotification;
    private User? _receivedUser;
    #endregion

    #region Properties
    [Column("received_notification_id")]
    [Key]
    public Guid ReceivedNotificationId { get; set; }

    [Column("received_user_id")]
    [Key]
    public Guid ReceivedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_receivedNotification))]
    public Notification ReceivedNotification
    {
        get => GetFieldOrThrowIfNull(_receivedNotification);
        set
        {
            ReceivedNotificationId = value.Id;
            _receivedNotification = value;
        }
    }

    [BackingField(nameof(_receivedUser))]
    public User ReceivedUser
    {
        get => GetFieldOrThrowIfNull(_receivedUser);
        set
        {
            ReceivedUserId = value.Id;
            _receivedUser = value;
        }
    }
    #endregion
}
