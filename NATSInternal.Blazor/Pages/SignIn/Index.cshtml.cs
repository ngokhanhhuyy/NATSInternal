using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NATSInternal.Blazor.Pages.SignIn;

public class SignIn : PageModel
{
    private readonly IAuthenticationService _service;
    private readonly IValidator<SignInRequestDto> _validator;

    [BindProperty]
    public SignInModel Model { get; set; } = new SignInModel();

    public SignIn(IAuthenticationService service, IValidator<SignInRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    public IActionResult OnGet()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return Redirect("/");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        SignInRequestDto requestDto = Model.ToRequestDto();
        ValidationResult validationResult = _validator.Validate(requestDto);
        if (!validationResult.IsValid)
        {
            ModelState.AddModelErrorsFromValidationErrors(validationResult.Errors);
            return Page();
        }

        try
        {
            await _service.SignInAsync(requestDto);
            return Redirect("/");
        }
        catch (OperationException exception)
        {
            ModelState.AddModelErrorsFromServiceException(exception);
            return Page();
        }
    }
}
