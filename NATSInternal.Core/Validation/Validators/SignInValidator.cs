namespace NATSInternal.Core.Validation.Validators;

internal class SignInValidator : Validator<SignInRequestDto>
{
    public SignInValidator()
    {
        RuleFor(dto => dto.UserName)
            .NotEmpty()
            .WithName(dto => DisplayNames.Get(nameof(dto.UserName)));
        RuleFor(dto => dto.Password)
            .NotEmpty()
            .WithName(dto => DisplayNames.Get(nameof(dto.Password)));
    }
}