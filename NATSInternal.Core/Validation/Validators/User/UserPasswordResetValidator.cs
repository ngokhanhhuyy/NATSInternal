namespace NATSInternal.Core.Validation.Validators;

internal class UserPasswordResetValidator : Validator<UserPasswordResetRequestDto>
{
    #region Constructors
    public UserPasswordResetValidator()
    {
        RuleFor(dto => dto.NewPassword)
            .NotEmpty()
            .Length(8, 20)
            .WithName(dto => DisplayNames.NewPassword);
        RuleFor(dto => dto.ConfirmationPassword)
            .NotEmpty()
            .Must((dto, confirmationPassword) => confirmationPassword == dto.NewPassword)
            .WithMessage(dto => ErrorMessages.MismatchedWith
                .Replace("{ComparisonPropertyName}", DisplayNames.NewPassword.ToLower()))
            .WithName(dto => DisplayNames.ConfirmationPassword);
    }
    #endregion
}
