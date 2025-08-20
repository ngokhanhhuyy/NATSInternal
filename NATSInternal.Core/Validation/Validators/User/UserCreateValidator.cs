namespace NATSInternal.Core.Validation.Validators;

internal class UserCreateValidator : Validator<UserCreateRequestDto>
{
    #region Constructors
    public UserCreateValidator()
    {
        RuleFor(dto => dto.UserName)
            .NotEmpty()
            .MaximumLength(20)
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage(ErrorMessages.InvalidUserNamePattern)
            .WithName(dto => DisplayNames.UserName);
        RuleFor(dto => dto.Password)
            .NotEmpty()
            .Length(8, 20)
            .WithName(dto => DisplayNames.Get(nameof(dto.Password)));
        RuleFor(dto => dto.ConfirmationPassword)
            .NotEmpty()
            .Must((dto, confirmationPassword) => dto.Password == confirmationPassword)
            .WithMessage(dto => ErrorMessages.MismatchedWith.Replace("{ComparisonPropertyName}", DisplayNames.Password))
            .WithName(dto => DisplayNames.Password);
    }
    #endregion
}
