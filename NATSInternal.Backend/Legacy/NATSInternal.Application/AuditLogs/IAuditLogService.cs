namespace NATSInternal.Application.AuditLogs;

internal interface IAuditLogService
{
    #region Methods
    Task LogUserCreateActionAsync(
        Guid id,
        UserSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogUserResetPasswordActionAsync(Guid id, DateTime loggedDateTime, CancellationToken token = default);

    Task LogUserAddToRolesActionAsync(
        Guid id,
        UserSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogUserRemoveFromRolesActionAsync(
        Guid id,
        UserSnapshot targetUserSnapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogUserRemoveActionAsync(Guid id, DateTime loggedDateTime, CancellationToken token = default);

    Task LogProductCreateActionAsync(
        Guid id,
        ProductSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);

    Task LogProductUpdateActionAsync(
        Guid id,
        ProductSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default);
    #endregion
}