using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Authentication;

public class VerifyUserNameAndPasswordRequestDto : IRequestDto
{
    #region Properties
    public required string UserName { get; set; }
    public required string Password { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}