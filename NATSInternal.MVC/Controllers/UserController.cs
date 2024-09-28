namespace NATSInternal.MVC.Controllers;

[Route("[controller]")]
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
        RoleListResponseDto roleListResponseDto = await _roleService.GetListAsync();
        UserListRequestDto requestDto = model.ToRequestDto().TransformValues();

        ValidationResult validationResult;
        validationResult = _listValidator.Validate(requestDto.TransformValues());
        if (!validationResult.IsValid)
        {
            model = new UserListModel();
        }

        UserListResponseDto userListResponseDto = await _userService.GetListAsync(requestDto);
        model.MapFromResponseDto(userListResponseDto, roleListResponseDto);

        return View(model);
    }
}
