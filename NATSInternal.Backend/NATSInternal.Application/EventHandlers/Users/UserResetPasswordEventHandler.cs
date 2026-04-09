using MediatR;
using NATSInternal.Application.AuditLogs;
using NATSInternal.Application.Time;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

internal class UserResetPasswordEventHandler : INotificationHandler<UserResetPasswordEvent>
{
    #region Fields
    private readonly IAuditLogService _auditLogService;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public UserResetPasswordEventHandler(IAuditLogService auditLogService, IClock clock)
    {
        _auditLogService = auditLogService;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task Handle(UserResetPasswordEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _auditLogService.LogUserResetPasswordActionAsync(domainEvent.Snapshot.Id, _clock.Now, cancellationToken);
    }
    #endregion
}
