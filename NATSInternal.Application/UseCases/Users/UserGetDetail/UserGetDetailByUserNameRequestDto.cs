using MediatR;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetDetailByUserNameRequestDto : IRequestDto, IRequest<UserGetDetailResponseDto>
{
    #region Properties
    public required string UserName { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}