using MediatR;
using NATSInternal.Application.AuditLogs;
using NATSInternal.Application.Time;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

internal class UserRemmovedFromRolesEventHandler : INotificationHandler<UserRemovedFromRoleEvent>
{
    #region Fields
    private readonly IAuditLogService _auditLogService;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public UserRemmovedFromRolesEventHandler(IAuditLogService auditLogService, IClock clock)
    {
        _auditLogService = auditLogService;
        _clock = clock;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserRemovedFromRoleEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _auditLogService.LogUserRemoveFromRolesActionAsync(
            domainEvent.BeforeRemovalSnapshot,
            domainEvent.AfterRemovalSnapshot,
            _clock.Now,
            cancellationToken
        );
    }
    #endregion
}