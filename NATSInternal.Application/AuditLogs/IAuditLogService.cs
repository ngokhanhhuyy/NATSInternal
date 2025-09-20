using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.AuditLogs;

internal interface IAuditLogService
{
    #region Methods
    Task LogUserCreateActionAsync(
        User targetUser,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogUserResetPasswordActionAsync(
        Guid targetUserId,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogUserAddToRolesActionAsync(
        Guid targetUserId,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);
    #endregion
}