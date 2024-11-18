namespace NATSInternal.MVC.Controllers;

[Route("[controller]")]
[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IValidator<UserListRequestDto> _listValidator;

    public UserController(
            IUserService userService,
            IRoleService roleService,
            IValidator<UserListRequestDto> listValidator)
    {
        _userService = userService;
        _roleService = roleService;
        _listValidator = listValidator;
    }

    [HttpGet]
    public async Task<IActionResult> UserList([FromQuery] UserListModel model)
    {
        UserListRequestDto requestDto = model.ToRequestDto();
        ValidationResult validationResult = _listValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            model = new UserListModel();
        }

        UserListResponseDto userListResponseDto = await _userService.GetListAsync(requestDto);

        UserListResponseDto joinedRecentlyUsersResponseDto;
        joinedRecentlyUsersResponseDto = await _userService.GetListAsync(new UserListRequestDto
        {
            SortingByField = nameof(OrderByFieldOption.Birthday),
            JoinedRencentlyOnly = true,
        });

        UserListResponseDto incomingBirthdayUsersResponseDto;
        incomingBirthdayUsersResponseDto = await _userService
            .GetListAsync(new UserListRequestDto
            {
                SortingByField = nameof(OrderByFieldOption.Birthday),
                JoinedRencentlyOnly = true,
            });

        List<RoleMinimalResponseDto> roleOptionsResponseDtos;
        roleOptionsResponseDtos = await _roleService.GetAllAsync();

        model.MapFromResponseDto(
            userListResponseDto,
            joinedRecentlyUsersResponseDto,
            incomingBirthdayUsersResponseDto,
            roleOptionsResponseDtos);

        return View("UserList/UserListView", model);
    }
}
