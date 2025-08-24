namespace NATSInternal.Core.Interfaces.Notifiers;

/// <summary>
/// A class to send notification to users who are connecting to the <c><see cref="ApplicationHub" /></c>.
/// </summary>
public interface INotifier
{
    /// <summary>
    /// Create a notification with the specified type and distribute it to the users who have been specifed to be the
    /// receivers of the notification and are connecting to the notification hub.
    /// </summary>
    /// <param name="notificationType">
    /// The type of the notification.
    /// </param>
    /// <param name="resourceIds">
    /// The ids of the resource if the notification type is to indicate that some resource has been interacted.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task Notify(
            NotificationType notificationType,
            Guid[]? resourceIds,
            CancellationToken cancellationToken = default);
}