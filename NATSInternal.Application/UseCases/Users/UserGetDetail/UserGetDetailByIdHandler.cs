using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

internal class UserGetDetailByIdHandler : AbstractUserGetDetailHandler<UserGetDetailByIdRequestDto>
{
    #region Constructors
    public UserGetDetailByIdHandler(
        IUserRepository repository,
        IAuthorizationService authorizationService) : base(repository, authorizationService) { }
    #endregion

    #region Methods
    public override async Task<UserGetDetailResponseDto> Handle(
        UserGetDetailByIdRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        return await Handle(
            async (repository, token) => await repository.GetUserByIdAsync(requestDto.Id, token),
            true,
            cancellationToken
        );
    }
    #endregion
}
