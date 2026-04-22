using NATSInternal.Application.AuditLogs;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.PersistenceModels;
using NATSInternal.Application.Security;
using System.Text.Json;

namespace NATSInternal.Infrastructure.Services;

internal class AuditLogService : IAuditLogService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly Guid _performedUserId;
    #endregion

    #region Constructors
    public AuditLogService(AppDbContext context, ICallerDetailProvider callerDetailProvider)
    {
        _context = context;
        _performedUserId = callerDetailProvider.GetId();
    }
    #endregion

    #region Methods
    public async Task LogUserCreateActionAsync(
        Guid id,
        UserSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken token = default)
    {
        AuditLog auditLog = new()
        {
            TargetResourceId = id,
            ActionName = AuditLogActionNames.UserResetPassword,
            LoggedDateTime = loggedDateTime,
            PerformedUserId = _performedUserId
        };

        auditLog.SetSnapshot(snapshot);
        await AddAndSaveAsync(auditLog, token);
    }

    public async Task LogUserResetPasswordActionAsync(
        Guid id,
        DateTime loggedDateTime,
        CancellationToken token = default)
    {
        AuditLog auditLog = new()
        {
            TargetResourceId = id,
            ActionName = AuditLogActionNames.UserResetPassword,
            LoggedDateTime = loggedDateTime,
            PerformedUserId = _performedUserId
        };

        await AddAndSaveAsync(auditLog, token);
    }

    public async Task LogUserAddToRolesActionAsync(
        Guid id,
        UserSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default)
    {
        AuditLog auditLog = new()
        {
            TargetResourceId = id,
            ActionName = AuditLogActionNames.UserAddToRoles,
            LoggedDateTime = loggedDateTime,
            PerformedUserId = _performedUserId
        };

        auditLog.SetSnapshot(snapshot);
        await AddAndSaveAsync(auditLog, cancellationToken);
    }

    public async Task LogUserRemoveFromRolesActionAsync(
        Guid id,
        UserSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken token = default)
    {
        AuditLog auditLog = new()
        {
            TargetResourceId = id,
            ActionName = AuditLogActionNames.UserRemoveFromRoles,
            LoggedDateTime = loggedDateTime,
            PerformedUserId = _performedUserId
        };

        auditLog.SetSnapshot(snapshot);
        await AddAndSaveAsync(auditLog, token);
    }

    public async Task LogUserRemoveActionAsync(Guid id, DateTime loggedDateTime, CancellationToken token = default)
    {
        AuditLog auditLog = new()
        {
            TargetResourceId = id,
            ActionName = AuditLogActionNames.UserRemove,
            LoggedDateTime = loggedDateTime,
            PerformedUserId = _performedUserId
        };

        await AddAndSaveAsync(auditLog, token);
    }

    public async Task LogProductCreateActionAsync(
        Guid id,
        ProductSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken token = default)
    {
        AuditLog auditLog = new()
        {
            TargetResourceId = id,
            ActionName = AuditLogActionNames.ProductCreate,
            LoggedDateTime = loggedDateTime,
            PerformedUserId = _performedUserId
        };

        auditLog.SetSnapshot(snapshot);
        await AddAndSaveAsync(auditLog, token);
    }

    public async Task LogProductUpdateActionAsync(
        Guid id,
        ProductSnapshot snapshot,
        DateTime loggedDateTime,
        CancellationToken token = default)
    {
        AuditLog auditLog = new()
        {
            TargetResourceId = id,
            ActionName = AuditLogActionNames.ProductCreate,
            LoggedDateTime = loggedDateTime,
            PerformedUserId = _performedUserId
        };

        auditLog.SetSnapshot(snapshot);
        await AddAndSaveAsync(auditLog, token);
    }
    #endregion

    #region PrivateMethods
    private async Task AddAndSaveAsync(AuditLog auditLog, CancellationToken cancellationToken)
    {
        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync(cancellationToken);
    }
    #endregion
}
