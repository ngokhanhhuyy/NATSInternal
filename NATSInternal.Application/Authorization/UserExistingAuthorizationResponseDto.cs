namespace NATSInternal.Application.Authorization;

public class UserExistingAuthorizationResponseDto
{
    #region Properties
    public required bool CanChangePassword { get; set; }
    public required bool CanResetPassword { get; set; }
    public required bool CanDelete { get; set; }
    public required bool CanAddToOrRemoveFromRoles { get; set; }
    #endregion
}