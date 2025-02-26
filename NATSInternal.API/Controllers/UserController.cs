﻿namespace NATSInternal.Controllers.Api;

[Route("/Api/User")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IValidator<UserListRequestDto> _listValidator;
    private readonly IValidator<UserCreateRequestDto> _createValidator;
    private readonly IValidator<UserUpdateRequestDto> _updateValidator;
    private readonly IValidator<UserPasswordChangeRequestDto> _passwordChangeValidator;
    private readonly IValidator<UserPasswordResetRequestDto> _passwordResetValidator;
    private readonly INotifier _notifier;

    public UserController(
            IUserService userService,
            IAuthorizationService authorizationService,
            IValidator<UserListRequestDto> listValidator,
            IValidator<UserCreateRequestDto> createValidator,
            IValidator<UserUpdateRequestDto> updateValidator,
            IValidator<UserPasswordChangeRequestDto> passwordChangeValidator,
            IValidator<UserPasswordResetRequestDto> passwordResetValidator,
            INotifier notifier)
    {
        _userService = userService;
        _authorizationService = authorizationService;
        _listValidator = listValidator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _passwordChangeValidator = passwordChangeValidator;
        _passwordResetValidator = passwordResetValidator;
        _notifier = notifier;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UserList([FromQuery] UserListRequestDto requestDto)
    {
        // Validate data from request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _listValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform fetching operation.
        UserListResponseDto responseDto = await _userService.GetListAsync(requestDto);
        return Ok(responseDto);
    }

    [HttpGet("JoinedRecently")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> JoinedRecentlyUserList()
    {
        UserListResponseDto responseDto = await _userService.GetJoinedRecentlyListAsync();
        return Ok(responseDto);
    }

    [HttpGet("UpcomingBirthday")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpcomingBirthdayUserList()
    {
        UserListResponseDto responseDto = await _userService.GetUpcomingBirthdayListAsync();
        return Ok(responseDto);
    }

    [HttpGet("{id:int}/Role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UserRole(int id)
    {
        try
        {
            return Ok(await _userService.GetRoleAsync(id));
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(exception);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UserDetail(int id)
    {
        try
        {
            UserDetailResponseDto responseDto;
            responseDto = await _userService.GetDetailAsync(id);
            return Ok(responseDto);
        }
        catch (ResourceNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("Caller")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CallerDetail()
    {
        int callerId = _authorizationService.GetUserId();
        UserDetailResponseDto responseDto = await _userService.GetDetailAsync(callerId);
        return Ok(responseDto);
    }

    [HttpPost]
    [Authorize(Policy = "CanCreateUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UserCreate([FromBody] UserCreateRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _createValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform creating operation.
        try
        {
            // Create the user.
            int createdId = await _userService.CreateAsync(requestDto);
            string createdResourceUrl = Url.Action("UserDetail", "User", createdId);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.UserCreation, createdId);

            return Created(createdResourceUrl, createdId);
        }
        catch (DuplicatedException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return Conflict(ModelState);
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (InvalidOperationException exception)
        {
            ModelState.Clear();
            ModelState.AddModelError("", exception.Message);
            return UnprocessableEntity(ModelState);
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateUser(
            int id,
            [FromBody] UserUpdateRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _updateValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the updating operation.
        try
        {
            // Update the user.
            await _userService.UpdateAsync(id, requestDto);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.UserModification, id);

            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ModelState);
        }
    }

    [HttpPut("ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeUserPassword(
            [FromBody] UserPasswordChangeRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _passwordChangeValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the password change operation.
        try
        {
            await _userService.ChangePasswordAsync(requestDto);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(ModelState);
        }
    }

    [HttpPut("{id:int}/ResetPassword")]
    [Authorize(Policy = "CanResetPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ResetUserPassword(
            int id,
            [FromBody] UserPasswordResetRequestDto requestDto)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        ValidationResult validationResult;
        validationResult = _passwordResetValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return BadRequest(ModelState);
        }

        // Perform the password reset operation.
        try
        {
            await _userService.ResetPasswordAsync(id, requestDto);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(exception);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "CanDeleteUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            // Delete the user.
            await _userService.DeleteAsync(id);

            // Create and distribute the notification to the users.
            await _notifier.Notify(NotificationType.UserDeletion, id);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return UnprocessableEntity(exception);
        }
    }

    [HttpPost("{id:int}/Restore")]
    [Authorize(Policy = "CanRestore")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RestoreUser(int id)
    {
        try
        {
            await _userService.RestoreAsync(id);
            return Ok();
        }
        catch (ResourceNotFoundException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return NotFound(ModelState);
        }
        catch (AuthorizationException)
        {
            return Forbid();
        }
    }

    [HttpGet("ListSortingOptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListSortingOptions()
    {
        return Ok(_userService.GetListSortingOptions());
    }

    [HttpGet("{id:int}/PasswordResetPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPasswordResetPermission(int id)
    {
        return Ok(await _userService.GetPasswordResetPermission(id));
    }

    [HttpGet("CreatingPermission")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCreatingPermission()
    {
        return Ok(_userService.GetCreatingPermission());
    }
}
