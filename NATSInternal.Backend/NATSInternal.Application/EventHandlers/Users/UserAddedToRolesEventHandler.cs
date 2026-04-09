using MediatR;
using NATSInternal.Application.AuditLogs;
using NATSInternal.Application.Time;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

internal class UserAddedToRolesEventHandler : INotificationHandler<UserAddedToRolesEvent>
{
    #region Fields
    private readonly IAuditLogService _auditLogService;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public UserAddedToRolesEventHandler(IAuditLogService auditLogService, IClock clock)
    {
        _auditLogService = auditLogService;
        _clock = clock;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserAddedToRolesEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _auditLogService.LogUserAddToRolesActionAsync(
            domainEvent.BeforeAddingSnapshot,
            domainEvent.AfterAddingSnapshot,
            _clock.Now,
            cancellationToken
        );
    }
    #endregion
}