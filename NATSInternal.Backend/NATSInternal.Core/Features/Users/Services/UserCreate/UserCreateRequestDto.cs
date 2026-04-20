namespace NATSInternal.Core.Features.Users;

public class UserCreateRequestDto
{
    #region Properties
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string ConfirmationPassword { get; set; }
    public required List<string> RoleNames { get; set; }
    #endregion
}