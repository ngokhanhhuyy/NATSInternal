using NATSInternal.Core.Features.Users;

namespace NATSInternal.Test.Mock;

public class CallerDetailContext
{
    #region Fields
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    #endregion

    #region Constructors
    public CallerDetailContext(IUserService userService, IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }

    public async Task<UserDetailResponseDto> GetCallerDetailByRoleNameAsync(string roleName)
    {
        List<RoleBasicResponseDto> roleResponseDtos = await _roleService.GetAllAsync();
        int roleId = roleResponseDtos.Where(r => r.Name == roleName).Select(r => r.Id).Single();
        UserListRequestDto requestDto = new()
        {
            RoleIds = new() { roleId }
        };

        UserListResponseDto listResponseDto = await _userService.GetListAsync(requestDto);
        int userId = listResponseDto.Items.OrderBy(_ => Guid.NewGuid()).Select(u => u.Id).First();

        return await _userService.GetDetailByIdAsync(userId);
    }
    #endregion
}