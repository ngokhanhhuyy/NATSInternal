namespace NATSInternal.Domain.Features.Users;

public class UserSnapshot
{
    #region Constructors
    internal UserSnapshot(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        RoleIds = user.Roles.Select(r => r.Id).ToList();
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string UserName { get; }
    public ICollection<Guid> RoleIds { get; }
    #endregion
}