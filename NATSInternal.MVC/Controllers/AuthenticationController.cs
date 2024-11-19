namespace NATSInternal.Controllers;

[Authorize]
public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidator<SignInRequestDto> _signInValidator;

    public AuthenticationController(
            IAuthenticationService authenticationService,
            IValidator<SignInRequestDto> signInValidator)
    {
        _authenticationService = authenticationService;
        _signInValidator = signInValidator;
    }

    [HttpGet("SignIn")]
    [AllowAnonymous]
    public IActionResult SignIn()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(new SignInModel());
    }

    [HttpPost("SignIn")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(
            [FromForm] SignInModel model,
            [FromQuery] string returningUrl)
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        SignInRequestDto requestDto = model.ToRequestDto();
        ValidationResult validationResult = _signInValidator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return View(model);
        }

        try
        {
            await _authenticationService.SignInAsync(model.ToRequestDto());
            if (returningUrl != null)
            {
                return Redirect(returningUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return View(model);
        }
    }

    [HttpPost("SignOut")]
    [ValidateAntiForgeryToken]
    public new async Task<IActionResult> SignOut()
    {
        await _authenticationService.SignOutAsync();
        return RedirectToAction("SignIn");
    }
}
