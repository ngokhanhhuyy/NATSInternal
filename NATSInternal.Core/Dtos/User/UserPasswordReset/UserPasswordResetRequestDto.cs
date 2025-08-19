namespace NATSInternal.Core.Dtos;

public class UserPasswordResetRequestDto : IRequestDto
{
    #region Properties
    public required string NewPassword { get; set; }
    public required string ConfirmationPassword { get; set; }
    #endregion

    public void TransformValues() { }
}
