using NATSInternal.Application.UseCases.Users;

namespace NATSInternal.Application.Services;

internal interface IUserService
{
    #region Methods
    Task<UserGetListResponseDto> GetPaginatedUserListAsync(
        UserGetListRequestDto requestDto,
        CancellationToken cancellationToken = default);
    #endregion
}