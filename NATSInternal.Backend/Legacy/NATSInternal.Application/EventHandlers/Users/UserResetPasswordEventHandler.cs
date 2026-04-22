using MediatR;
using NATSInternal.Application.AuditLogs;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.EventHandlers.Users;

internal class UserResetPasswordEventHandler : INotificationHandler<UserResetPasswordEvent>
{
    #region Fields
    private readonly IAuditLogService _auditLogService;
    #endregion

    #region Constructors
    public UserResetPasswordEventHandler(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }
    #endregion

    #region Methods
    public async Task Handle(UserResetPasswordEvent domainEvent, CancellationToken token = default)
    {
        await _auditLogService.LogUserResetPasswordActionAsync(
            domainEvent.Id,
            domainEvent.ResettedDateTime,
            token
        );
    }
    #endregion
}
