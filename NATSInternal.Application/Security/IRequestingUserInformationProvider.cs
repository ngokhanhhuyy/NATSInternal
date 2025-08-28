namespace NATSInternal.Application.Security;

public interface IRequestingUserInformationProvider
{
    #region Methods
    Guid GetRequestingUserId();
    string GetRequestingUserUserName();
    ICollection<string> GetRequestingUserRoleNames();
    ICollection<string> GetRequestingUserPermissionNames();
    #endregion
}
