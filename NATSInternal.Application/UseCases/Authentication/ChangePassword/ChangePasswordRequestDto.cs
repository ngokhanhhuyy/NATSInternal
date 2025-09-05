using MediatR;

namespace NATSInternal.Application.UseCases.Authentication;

public class ChangePasswordRequestDto : IRequestDto, IRequest
{
    #region Properties
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmationPassword { get; set; } = string.Empty;
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}