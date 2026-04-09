using MediatR;
using NATSInternal.Application.AuditLogs;
using NATSInternal.Application.Time;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

internal class UserCreateEventHandler : INotificationHandler<UserCreatedEvent>
{
    #region Fields
    private readonly IAuditLogService _auditLogService;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public UserCreateEventHandler(IAuditLogService auditLogService, IClock clock)
    {
        _auditLogService = auditLogService;
        _clock = clock;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _auditLogService.LogUserCreateActionAsync(
            domainEvent.Snapshot,
            _clock.Now,
            cancellationToken
        );
    }
    #endregion
}