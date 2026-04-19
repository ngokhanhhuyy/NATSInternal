using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.AuditLogs;

public class UserSnapshot
{
    #region Constructors
    internal UserSnapshot(User user)
    {
        UserName = user.UserName;
        RoleIds = user.Roles.Select(r => r.Id).ToList();
    }
    #endregion

    #region Properties
    public string UserName { get; }
    public ICollection<Guid> RoleIds { get; }
    #endregion
}