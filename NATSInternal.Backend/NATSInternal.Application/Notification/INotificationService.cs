namespace NATSInternal.Application.Notification;

public interface INotificationService
{
    #region Methods
    Task SendAsync(
        NotificationType type,
        Guid resourceId,
        DateTime occuredDateTime,
        CancellationToken cancellationToken = default);
    #endregion
}