using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

internal class UserGetDetailByUserNameHandler : AbstractUserGetDetailHandler<UserGetDetailByUserNameRequestDto>
{
    #region Constructors
    public UserGetDetailByUserNameHandler(
        IUserRepository repository,
        IAuthorizationInternalService authorizationInternalService) : base(repository, authorizationInternalService) { }
    #endregion

    #region Methods
    public override async Task<UserGetDetailResponseDto> Handle(
        UserGetDetailByUserNameRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        return await Handle(
            async (repository, token) => await repository.GetUserByUserNameAsync(requestDto.UserName, token),
            requestDto.IncludingAuthorization,
            cancellationToken
        );
    }
    #endregion
}