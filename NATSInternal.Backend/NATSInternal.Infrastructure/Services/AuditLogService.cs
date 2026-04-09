using NATSInternal.Application.AuditLogs;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Features.AuditLogs;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Application.Security;
using System.Text.Json;
using NATSInternal.Domain.Features.Products;

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
        UserSnapshot targetUserSnapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default)
    {
        AuditLog auditLog = new(
            targetUserSnapshot.Id,
            _performedUserId,
            AuditLogActionNames.UserCreate,
            null,
            JsonSerializer.Serialize(targetUserSnapshot),
            loggedDateTime
        );

        await AddAndSaveAsync(auditLog, cancellationToken);
    }

    public async Task LogUserResetPasswordActionAsync(
        Guid targetUserId,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default)
    {
        AuditLog auditLog = new(
            targetUserId,
            _performedUserId,
            AuditLogActionNames.UserResetPassword,
            null,
            null,
            loggedDateTime
        );

        await AddAndSaveAsync(auditLog, cancellationToken);
        
    }

    public async Task LogUserAddToRolesActionAsync(
        UserSnapshot targetUserBeforeAddingSnapshot,
        UserSnapshot targetUserAfterAddingSnapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default)
    {
        AuditLog auditLog = new(
            targetUserBeforeAddingSnapshot.Id,
            _performedUserId,
            AuditLogActionNames.UserAddToRoles,
            JsonSerializer.Serialize(targetUserBeforeAddingSnapshot),
            JsonSerializer.Serialize(targetUserAfterAddingSnapshot),
            loggedDateTime
        );

        await AddAndSaveAsync(auditLog, cancellationToken);
    }

    public async Task LogUserRemoveFromRolesActionAsync(
        UserSnapshot targetUserBeforeRemovalSnapshot,
        UserSnapshot targetUserAfterRemovalSnapshot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default)
    {
        AuditLog auditLog = new(
            targetUserBeforeRemovalSnapshot.Id,
            _performedUserId,
            AuditLogActionNames.UserRemoveFromRoles,
            JsonSerializer.Serialize(targetUserBeforeRemovalSnapshot),
            JsonSerializer.Serialize(targetUserAfterRemovalSnapshot),
            loggedDateTime
        );

        await AddAndSaveAsync(auditLog, cancellationToken);
    }

    public async Task LogProductCreateActionAsync(
        ProductSnapshot productSnapsnot,
        DateTime loggedDateTime,
        CancellationToken cancellationToken)
    {
        AuditLog auditLog = new(
            productSnapsnot.Id,
            _performedUserId,
            AuditLogActionNames.ProductCreate,
            null,
            JsonSerializer.Serialize(productSnapsnot),
            loggedDateTime
        );

        await AddAndSaveAsync(auditLog, cancellationToken);
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
