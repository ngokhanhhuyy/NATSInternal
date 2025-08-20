namespace NATSInternal.Core.Dtos;

public class UserCreateRequestDto : IRequestDto
{
    #region Properties
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string ConfirmationPassword { get; set; }
    public required List<string> RoleNames { get; set; } = new();
    #endregion

    #region Constructors
    public void TransformValues() { }
    #endregion
}