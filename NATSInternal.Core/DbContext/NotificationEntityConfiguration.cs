namespace NATSInternal.Core.DbContext;

internal class NotificationEntityConfiguration : IEntityTypeConfiguration<Notification>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Notification> entityBuilder)
    {
        entityBuilder.HasKey(n => n.Id);
        entityBuilder
            .Property(n => n.ResourceIds)
            .HasColumnType("JSON")
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<List<Guid>>(v, JsonSerializerOptions.Default)!);
        entityBuilder
            .HasOne(n => n.CreatedUser)
            .WithMany(u => u.CreatedNotifications)
            .HasForeignKey(n => n.CreatedUserId)
            .HasConstraintName("FK__notifications__users__created_user_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasMany(n => n.ReceivedUsers)
            .WithMany(u => u.ReceivedNotifications)
            .UsingEntity<NotificationReceivedUser>(
                notificationReceivedUser => notificationReceivedUser
                    .HasOne(nru => nru.ReceivedUser)
                    .WithMany()
                    .HasForeignKey(nru => nru.ReceivedUserId)
                    .HasConstraintName("FK__notification_received_users__users__received_user_id")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                notificationReceivedUser => notificationReceivedUser
                    .HasOne(nru => nru.ReceivedNotification)
                    .WithMany()
                    .HasForeignKey(nru => nru.ReceivedNotificationId)
                    .HasConstraintName("FK__notification_received_users__notifications__received_notification_id")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                notificationReceivedUser => notificationReceivedUser
                    .HasKey(nru => new { nru.ReceivedUserId, nru.ReceivedNotificationId }));
        entityBuilder
            .HasMany(n => n.ReadUsers)
            .WithMany(u => u.ReadNotifications)
            .UsingEntity<NotificationReadUser>(
                notificationReadUser => notificationReadUser
                    .HasOne(nru => nru.ReadUser)
                    .WithMany()
                    .HasForeignKey(nru => nru.ReadUserId)
                    .HasConstraintName("FK__notification_read_users__users__read_user_id")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                ,
                notificationReadUser => notificationReadUser
                    .HasOne(nru => nru.ReadNotification)
                    .WithMany()
                    .HasForeignKey(nru => nru.ReadNotificationId)
                    .HasConstraintName("FK__notification_read_users__notifications__read_notification_id")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                notificationReadUser => notificationReadUser
                    .HasKey(nru => new { nru.ReadUserId, nru.ReadNotificationId }));
    }
    #endregion
}
