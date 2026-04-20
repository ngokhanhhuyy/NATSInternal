namespace NATSInternal.Core.Common.Security;

public interface ICallerDetailProvider
{
    #region Methods
    int GetId();
    string GetUserName();
    ICollection<string> GetRoleNames();
    ICollection<string> GetPermissionNames();
    int GetPowerLevel();
    #endregion
}
