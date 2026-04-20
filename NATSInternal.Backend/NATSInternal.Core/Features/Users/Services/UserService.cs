using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Authorization;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;

namespace NATSInternal.Core.Features.Users;

internal class UserService : IUserService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationInternalService;
    private readonly IDbExceptionHandler _exceptionHandler;
    #endregion
    
    #region Constructors
    public UserService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationInternalService,
        IDbExceptionHandler exceptionHandler)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationInternalService = authorizationInternalService;
        _exceptionHandler = exceptionHandler;
    }
    #endregion
    
    #region Methods
    public async Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto)
    {
        IQueryable<User> query = _context.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.DeletedDateTime != null)
            .Where(u => u.Roles.Select(r => r.Id).Any(rid => requestDto.RoleIds.Contains(rid)));

        if (!string.IsNullOrEmpty(requestDto.SearchContent))
        {
            query = query.Where(u => requestDto.SearchContent.ToLower().Contains(u.UserName.ToLower()));
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(UserListRequestDto.FieldToSort.UserName):
                query = query.ApplySorting(u => u.UserName, requestDto.SortByAscending);
                break;
            case nameof(UserListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(u => u.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(UserListRequestDto.FieldToSort.RoleMaxPowerLevel):
                query = query.ApplySorting(u => u.Roles.Max(r => r.PowerLevel), requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        Page<User> queryResult = await _listFetchingService.GetPagedListAsync(
            query,
            requestDto.Page,
            requestDto.ResultsPerPage
        );

        List<UserBasicResponseDto> userResponseDtos = queryResult.Items
            .Select(u => new UserBasicResponseDto(u, _authorizationInternalService.GetUserExistingAuthorization(u)))
            .ToList();

        return new(userResponseDtos, queryResult.PageCount, queryResult.ItemCount);
    }

    public async Task<UserDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == id)
            .Select(u => new UserDetailResponseDto(u, _authorizationInternalService.GetUserExistingAuthorization(u)))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    public async Task<int> CreateAsync(UserCreateRequestDto requestDto)
    {
        
    }
    #endregion
}