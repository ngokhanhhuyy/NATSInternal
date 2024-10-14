using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NATSInternal.Services.Entities;

internal class Notification : IUpsertableEntity<Notification>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public NotificationType Type { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; }
    
    public List<int> ResourceIds { get; set; }
    
    // Foreign key.
    public int? CreatedUserId { get; set; }

    // Navigation properties.
    public virtual User CreatedUser { get; set; }
    public virtual List<User> ReceivedUsers { get; set; }
    public virtual List<User> ReadUsers { get; set; }
    
    // Computed properies.
    [NotMapped]
    public int ResourcePrimaryId => ResourceIds[0];
    
    [NotMapped]
    public int ResourceSecondaryId => ResourceIds[1];
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<Notification> entityBuilder)
    {
        entityBuilder.HasKey(n => n.Id);
        entityBuilder.Property(n => n.ResourceIds)
            .HasConversion(new ValueConverter<List<int>, string>(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<int>>(
                    v,
                    JsonSerializerOptions.Default)))
            .HasColumnType("JSON");
        entityBuilder.HasOne(n => n.CreatedUser)
            .WithMany(u => u.CreatedNotifications)
            .HasForeignKey(n => n.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasMany(n => n.ReceivedUsers)
            .WithMany(u => u.ReceivedNotifications)
            .UsingEntity<NotificationReceivedUser>(
                notificationReceivedUser => notificationReceivedUser
                    .HasOne(nru => nru.ReceivedUser)
                    .WithMany()
                    .HasForeignKey(nru => nru.ReceivedUserId),
                notificationReceivedUser => notificationReceivedUser
                    .HasOne(nru => nru.ReceivedNotification)
                    .WithMany()
                    .HasForeignKey(nru => nru.ReceivedNotificationId));
        entityBuilder.HasMany(n => n.ReadUsers)
            .WithMany(u => u.ReadNotifications)
            .UsingEntity<NotificationReadUser>(
                notificationReadUser => notificationReadUser
                    .HasOne(nru => nru.ReadUser)
                    .WithMany()
                    .HasForeignKey(nru => nru.ReadUserId)
                ,
                notificationReceivedUser => notificationReceivedUser
                    .HasOne(nru => nru.ReadNotification)
                    .WithMany()
                    .HasForeignKey(nru => nru.ReadNotificationId));
    }
}
