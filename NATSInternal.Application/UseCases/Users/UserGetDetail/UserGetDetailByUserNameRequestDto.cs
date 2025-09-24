using MediatR;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetDetailByUserNameRequestDto : IRequestDto, IRequest<UserGetDetailResponseDto>
{
    #region Properties
    public required string UserName { get; set; }
    public bool IncludingAuthorization { get; set; } = true;
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}