using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NATSInternal.Blazor.Pages;

[BindProperties]
public class SignInPageModel : PageModel
{
    private readonly IAuthenticationService _service;
    private readonly IValidator<SignInRequestDto> _validator;

    [Display(Name = DisplayNames.UserName)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Password)]
    public string Password { get; set; }

    public SignInPageModel(
            IAuthenticationService service,
            IValidator<SignInRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        SignInRequestDto requestDto = ToRequestDto();
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

    public SignInRequestDto ToRequestDto()
    {
        SignInRequestDto requestDto = new SignInRequestDto
        {
            UserName = UserName,
            Password = Password
        };
        requestDto.TransformValues();
        return requestDto;
    }
}