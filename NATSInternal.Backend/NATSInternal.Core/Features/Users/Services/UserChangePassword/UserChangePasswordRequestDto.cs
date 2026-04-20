namespace NATSInternal.Core.Features.Users;

public class UserChangePasswordRequestDto
{
    #region Properties
    public required string NewPassword { get; set; }
    public required string ConfirmationPassword { get; set; }
    #endregion
}