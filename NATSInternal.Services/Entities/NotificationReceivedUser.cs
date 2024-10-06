namespace NATSInternal.Services.Entities;

internal class NotificationReceivedUser : IEntity<NotificationReceivedUser>
{
    [Key]
    public int ReceivedNotificationId { get; set; }

    [Key]
    public int ReceivedUserId { get; set; }

    // Navigation properties.
    public virtual Notification ReceivedNotification { get; set; }
    public virtual User ReceivedUser { get; set; }
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<NotificationReceivedUser> builder)
    {
        builder.HasKey(nru => new { nru.ReceivedNotificationId, nru.ReceivedUserId });
    }
}
