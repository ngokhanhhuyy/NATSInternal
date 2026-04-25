namespace NATSInternal.Core.Features.Authorization;

public class UserExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanChangePassword { get; internal set; }
    public bool CanResetPassword { get; internal set; }
    public bool CanUpdate { get; internal set; }
    public bool CanDelete { get; internal set; }
    #endregion
}