using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Shared;

namespace NATSInternal.Application.UseCases.Users;

internal class UserGetListHandler : IRequestHandler<UserGetListRequestDto, UserGetListResponseDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IAuthorizationService _authorizationService;
    #endregion

    #region Constructors
    public UserGetListHandler(IUserRepository repository, IAuthorizationService authorizationService)
    {
        _repository = repository;
        _authorizationService = authorizationService;
    }
    #endregion

    #region Methods
    public async Task<UserGetListResponseDto> Handle(UserGetListRequestDto requestDto, CancellationToken cancellationToken)
    {
        Page<User> page = await _repository.GetUserListAsync(
            requestDto.SortByAscending,
            requestDto.SortByFieldName,
            requestDto.Page,
            requestDto.ResultsPerPage,
            requestDto.RoleId,
            requestDto.SearchContent,
            cancellationToken
        );

        return new(page, _authorizationService.GetUserExistingAuthorization);
    }
    #endregion
}