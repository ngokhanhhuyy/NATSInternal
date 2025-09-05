using JetBrains.Annotations;
using NATSInternal.Application.Security;
using NATSInternal.Core.Constants;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.Authorization;

[UsedImplicitly]
internal class AuthorizationService : IAuthorizationService
{
    #region Fields
    private readonly ICallerDetailProvider _callerDetailProvider;
    #endregion

    #region Constructors
    public AuthorizationService(ICallerDetailProvider callerDetailProvider)
    {
        _callerDetailProvider = callerDetailProvider;
    }
    #endregion

    #region Methods
    public UserExistingAuthorizationResponseDto GetUserExistingAuthorization(User user)
    {
        return new()
        {
            CanChangePassword = CanChangeUserPassword(user),
            CanResetPassword = CanResetUserPassword(user),
            CanDelete = CanDeleteUser(user),
            CanAddToOrRemoveFromRoles = CanAddUserToOrRemoveUserFromRoles(user)
        };
    }

    public bool CanCreateUser()
    {
        return CallerHasPermission(PermissionNames.CreateUser);
    }

    public bool CanChangeUserPassword(User user)
    {
        return user.Id == _callerDetailProvider.GetId();
    }

    public bool CanResetUserPassword(User user)
    {
        return user.Id != _callerDetailProvider.GetId() && CallerHasPermission(PermissionNames.ResetOtherUserPassword);
    }

    public bool CanDeleteUser(User user)
    {
        return user.Id != _callerDetailProvider.GetId() && CallerHasPermission(PermissionNames.DeleteUser);
    }

    public bool CanAddUserToOrRemoveUserFromRoles(User user)
    {
        return user.Id != _callerDetailProvider.GetId() &&
            CallerHasPermission(PermissionNames.AddUserToOrRemoveUserFromRoles);
    }

    public bool CanAddUserToRole(User user, Role role)
    {
        return CanAddUserToOrRemoveUserFromRoles(user)
            && CallerPowerLevel() > user.PowerLevel
            && CallerPowerLevel() >= role.PowerLevel;
    }

    public bool CanRemoveUserFromRole(User user, Role role)
    {
        return CanAddUserToOrRemoveUserFromRoles(user)
            && CallerPowerLevel() > user.PowerLevel
            && CallerPowerLevel() > role.PowerLevel;
    }
    #endregion

    #region PrivateMethods
    private bool CallerHasPermission(string permissionName)
    {
        return _callerDetailProvider.GetPermissionNames().Contains(permissionName);
    }

    private int CallerPowerLevel()
    {
        return _callerDetailProvider.GetPowerLevel();
    }
    #endregion
}