namespace NATSInternal.Services.Entities;

internal class NotificationReadUser : IEntity<NotificationReadUser>
{
    [Key]
    public int ReadNotificationId { get; set; }

    [Key]
    public int ReadUserId { get; set; }

    // Navigation properties.
    public virtual Notification ReadNotification { get; set; }
    public virtual User ReadUser { get; set; }
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<NotificationReadUser> entityBuilder)
    {
        entityBuilder.HasKey(nru => new { nru.ReadNotificationId, nru.ReadUserId });
    }
}
