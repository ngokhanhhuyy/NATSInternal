using NATSInternal.Application.Notification;

namespace NATSInternal.Api;

public class NotificationService : INotificationService
{
    #region Methods
    public Task SendAsync(
        NotificationType type,
        Guid resourceId,
        DateTime occuredDateTime,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
    #endregion
}

