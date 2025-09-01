using MediatR;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetDetailByIdRequestDto : IRequestDto, IRequest<UserGetDetailResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}
