using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Application.Services;
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
    private readonly IAuthorizationInternalService _authorizationInternalService;
    #endregion

    #region Constructors
    public UserService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationInternalService)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationInternalService = authorizationInternalService;
    }
    #endregion

    #region Methods
    public async Task<UserGetListResponseDto> GetPaginatedUserListAsync(
        UserGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = _context.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.DeletedDateTime != null);

        if (requestDto.RoleId.HasValue)
        {
            query = query.Where(u => u.Roles.Select(r => r.Id).Contains(requestDto.RoleId.Value));
        }

        if (requestDto.SearchContent is not null && requestDto.SearchContent.Length > 0)
        {
            query = query.Where(u => requestDto.SearchContent.ToLower().Contains(u.UserName.ToLower()));
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(UserGetListRequestDto.FieldToSort.UserName):
                query = query.ApplySorting(u => u.UserName, requestDto.SortByAscending);
                break;
            case nameof(UserGetListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(u => u.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(UserGetListRequestDto.FieldToSort.RoleMaxPowerLevel):
                query = query.ApplySorting(u => u.Roles.Max(r => r.PowerLevel), requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        Page<User> queryResult = await _listFetchingService.GetPagedListAsync(
            query,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        List<UserGetListUserResponseDto> userResponseDtos = queryResult.Items
            .Select(u => new UserGetListUserResponseDto(
                u,
                _authorizationInternalService.GetUserExistingAuthorization(u)))
            .ToList();

        return new(userResponseDtos, queryResult.PageCount, queryResult.ItemCount);
    }
    #endregion
}