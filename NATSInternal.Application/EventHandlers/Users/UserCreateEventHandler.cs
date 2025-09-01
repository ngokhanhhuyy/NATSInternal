using MediatR;
using NATSInternal.Application.Notification;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

internal class UserCreateEventHandler : INotificationHandler<UserCreateEvent>
{
    #region Fields
    public readonly INotificationService _notificationService;
    #endregion

    #region Constructors
    public UserCreateEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserCreateEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _notificationService.SendAsync(
            NotificationType.UserCreation,
            domainEvent.CreatedUserId,
            domainEvent.CreatedDateTime,
            cancellationToken
        );
    }
    #endregion
}