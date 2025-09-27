using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Application.UseCases.Users;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Extensions;
using NATSInternal.Application.Authorization;

namespace NATSInternal.Infrastructure.Services;

internal class UserService : IUserService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationService _authorizationService;
    #endregion

    #region Constructors
    public UserService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationService authorizationService)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationService = authorizationService;
    }
    #endregion

    #region Methods
    public async Task<UserGetListResponseDto> GetPaginatedUserListAsync(
        UserGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = _context.Users.Include(u => u.Roles).ThenInclude(r => r.Permissions);

        if (requestDto.RoleId.HasValue)
        {
            query = query.Where(u => u.Roles.Select(r => r.Id).Contains(requestDto.RoleId.Value));
        }

        if (requestDto.SearchContent is not null && requestDto.SearchContent.Length > 0)
        {
            query = query.Where(u => requestDto.SearchContent.ToLower().Contains(u.UserName.ToLower()));
        }

        bool sortByAscendingOrDefault = requestDto.SortByAscending ?? true;
        switch (requestDto.SortByFieldName)
        {
            case nameof(UserGetListRequestDto.FieldToSort.UserName):
                query = query.ApplySorting(u => u.UserName, sortByAscendingOrDefault);
                break;
            case nameof(UserGetListRequestDto.FieldToSort.CreatedDateTime) or null:
                query = query.ApplySorting(u => u.CreatedDateTime, sortByAscendingOrDefault);
                break;
            case nameof(UserGetListRequestDto.FieldToSort.RoleMaxPowerLevel):
                query = query.ApplySorting(
                    u => u.Roles.Max(r => r.PowerLevel),
                    sortByAscendingOrDefault);
                break;
            default:
                throw new NotImplementedException();
        }

        Page<User> userPage = await _listFetchingService.GetPagedListAsync(
            query,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        List<UserBasicResponseDto> userResponseDtos = userPage.Items
            .Select(u => new UserBasicResponseDto(u, _authorizationService.GetUserExistingAuthorization(u)))
            .ToList();

        return new(userResponseDtos, userPage.PageCount);
    }
    #endregion
}