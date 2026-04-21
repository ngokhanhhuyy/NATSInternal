using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Authentication;

public class ResetPasswordRequestDto : IRequestDto
{
    #region Properties
    public required string NewPassword { get; set; }
    public required string ConfirmationPassword { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}