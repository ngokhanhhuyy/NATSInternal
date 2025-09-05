using MediatR;

namespace NATSInternal.Application.UseCases.Users;

public class UserResetPasswordRequestDto : IRequestDto, IRequest
{
    #region Properties
    public Guid Id { get; set; }
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmationPassword { get; set; } = string.Empty;
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}