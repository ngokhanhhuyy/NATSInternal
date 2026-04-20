namespace NATSInternal.Core.Features.Users;

public class UserResetPasswordRequestDto
{
    #region Properties
    public required string NewPassword { get; set; }
    #endregion
}