using NATSInternal.Application.AuditLogs;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Features.AuditLogs;
using NATSInternal.Infrastructure.DbContext;
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
        User targetUser,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default)
    {
        UserChangeDataDto afterModificationDataDto = new(targetUser);

        AuditLog auditLog = new(
            targetUser.Id,
            _performedUserId,
            AuditLogActionNames.UserCreate,
            null,
            JsonSerializer.Serialize(afterModificationDataDto),
            loggedDateTime
        );

        await AddAndSaveAsync(auditLog, cancellationToken);
    }

    public async Task LogUserResetPasswordAction(
        User targetUser,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default)
    {
        AuditLog auditLog = new(
            targetUser.Id,
            _performedUserId,
            AuditLogActionNames.UserResetPassword,
            null,
            null,
            loggedDateTime
        );

        await AddAndSaveAsync(auditLog, cancellationToken);
        
    }

    public async Task LogUserAddToRolesActionAsync(
        Guid targetUserId,
        DateTime loggedDateTime,
        CancellationToken cancellationToken = default)
    {
        AuditLog auditLog = new(
            targetUser.Id,
            _performedUserId,
            AuditLogActionNames.UserCreate,
            null,
            JsonSerializer.Serialize(afterModificationDataDto),
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
