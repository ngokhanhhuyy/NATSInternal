using MediatR;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Shared;

namespace NATSInternal.Application.UseCases.Users;

public class UserListHandler : IRequestHandler<UserListRequestDto, UserListResponseDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    #endregion

    #region Constructors
    public UserListHandler(IUserRepository repository)
    {
        _repository = repository;
    }
    #endregion

    #region Methods
    public async Task<UserListResponseDto> Handle(UserListRequestDto requestDto, CancellationToken cancellationToken)
    {
        Page<User> page = await _repository.GetListWithRolesAsync(
            requestDto.SortByAscending,
            requestDto.SortByFieldName,
            requestDto.Page,
            requestDto.ResultsPerPage,
            requestDto.RoleId,
            requestDto.SearchContent,
            cancellationToken
        );

        return new(page);
    }
    #endregion
}