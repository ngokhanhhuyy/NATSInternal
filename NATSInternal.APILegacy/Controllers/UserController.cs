namespace NATSInternal.Controllers.Api;

[Route("/Api/User")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    #region Fields
    private readonly IUserService _userService;
    private readonly IAuthorizationService _authorizationService;
    private readonly INotifier _notifier;
    #endregion

    #region Constructors
    public UserController(IUserService userService, IAuthorizationService authorizationService, INotifier notifier)
    {
        _userService = userService;
        _authorizationService = authorizationService;
        _notifier = notifier;
    }
    #endregion

    #region Methods
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UserList(
            [FromQuery] UserListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.GetListAsync(requestDto, cancellationToken));
    }

    [HttpGet("{id:guid}/Role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Role(Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.GetRoleAsync(id, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.GetDetailAsync(id, cancellationToken));
    }

    [HttpGet("Caller")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CallerDetail(CancellationToken cancellationToken = default)
    {
        Guid callerId = _authorizationService.GetUserId();
        return Ok(await _userService.GetDetailAsync(callerId, cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(
            [FromBody] UserCreateRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        Guid id = await _userService.CreateAsync(requestDto, cancellationToken);
        await _notifier.Notify(NotificationType.UserCreation, id);

        return CreatedAtAction(nameof(Detail), new { id }, id);
    }

    [HttpPut("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeUserPassword(
            [FromBody] UserPasswordChangeRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await _userService.ChangePasswordAsync(requestDto, cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}/ResetPassword")]
    [Authorize(Policy = "CanResetPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ResetUserPassword(
            Guid id,
            [FromBody] UserPasswordResetRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await _userService.ResetPasswordAsync(id, requestDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "CanDeleteUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
    {
        await _userService.DeleteAsync(id, cancellationToken);
        await _notifier.Notify(NotificationType.UserDeletion, id);
        return Ok();
    }

    [HttpGet("ListSortingOptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListSortingOptions()
    {
        return Ok(_userService.GetListSortingOptions());
    }

    [HttpGet("{id:guid}/PasswordResetPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPasswordResetPermission(Guid id)
    {
        return Ok(await _userService.GetPasswordResetPermission(id));
    }

    [HttpGet("CreatingPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCreatingPermission()
    {
        return Ok(_userService.GetCreatingPermission());
    }
    #endregion
}
