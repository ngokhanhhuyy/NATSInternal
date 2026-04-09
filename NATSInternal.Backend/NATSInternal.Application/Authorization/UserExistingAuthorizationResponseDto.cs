namespace NATSInternal.Application.Authorization;

public class UserExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanChangePassword { get; internal set; }
    public bool CanResetPassword { get; internal set; }
    public bool CanDelete { get; internal set; }
    public bool CanAddToOrRemoveFromRoles { get; internal set; }
    #endregion
}