namespace NATSInternal.Core.Security;

public interface ICallerDetailProvider
{
    #region Methods
    Guid GetId();
    string GetUserName();
    ICollection<string> GetRoleNames();
    ICollection<string> GetPermissionNames();
    int GetPowerLevel();
    #endregion
}
