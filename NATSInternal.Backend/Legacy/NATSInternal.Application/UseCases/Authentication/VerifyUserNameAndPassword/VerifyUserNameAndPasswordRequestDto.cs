using MediatR;

namespace NATSInternal.Application.UseCases.Authentication;

public class VerifyUserNameAndPasswordRequestDto : IRequestDto, IRequest
{
    #region Properties
    public required string UserName { get; set; }
    public required string Password { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}