using MediatR;
using NATSInternal.Application.Notification;
using NATSInternal.Application.Time;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

public class UserAddedToRoleEventHandler : INotificationHandler<UserAddedToRoleEvent>
{
    #region Fields
    private readonly INotificationService _notificationService;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public UserAddedToRoleEventHandler(INotificationService notificationService, IClock clock)
    {
        _notificationService = notificationService;
        _clock = clock;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserAddedToRoleEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _notificationService.SendAsync(
            NotificationType.UserAddToRole,
            domainEvent.AddedUserId,
            _clock.Now,
            cancellationToken
        );
    }
    #endregion
}