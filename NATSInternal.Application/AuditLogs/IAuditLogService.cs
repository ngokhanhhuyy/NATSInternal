using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.AuditLogs;

internal interface IAuditLogService
{
    #region Methods
    Task LogUserCreateActionAsync(
        UserSnapshot targetUserSnapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogUserResetPasswordActionAsync(
        Guid targetUserId,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogUserAddToRolesActionAsync(
        UserSnapshot targetUserBeforeAddingSnapshot,
        UserSnapshot targetUserAfterAddingSnapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogUserRemoveFromRolesActionAsync(
        UserSnapshot targetUserBeforeRemovalSnapshot,
        UserSnapshot targetUserAfterRemovalSnapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogProductCreateActionAsync();
    #endregion
}