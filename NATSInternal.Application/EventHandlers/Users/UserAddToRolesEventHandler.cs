using MediatR;
using NATSInternal.Application.Notification;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

public class UserAddToRolesEventHandler : INotificationHandler<UserAddToRolesEvent>
{
    #region Fields
    public readonly INotificationService _notificationService;
    #endregion

    #region Constructors
    public UserAddToRolesEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserAddToRolesEventHandler domainEvent, CancellationToken cancellationToken = default)
    {
        await _notificationService.SendAsync(
            NotificationType.UserAddToRoles,
            domainEvent.CreatedUserId,
            domainEvent.CreatedDateTime,
            cancellationToken
        );
    }
    #endregion
}