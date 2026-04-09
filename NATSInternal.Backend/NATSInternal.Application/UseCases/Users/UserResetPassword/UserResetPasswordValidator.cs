using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Rules;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Users;

[UsedImplicitly]
internal class UserResetPasswordValidator : Validator<UserResetPasswordRequestDto>
{
    #region Constructors
    public UserResetPasswordValidator()
    {
        RuleFor(dto => dto.NewPassword)
            .IsValidPassword()
            .WithName(DisplayNames.NewPassword);
        RuleFor(dto => dto.ConfirmationPassword)
            .NotEmpty()
            .Must((dto, confirmationPassword) => confirmationPassword == dto.NewPassword)
            .WithMessage(ErrorMessages.MismatchedWith
                .Replace("{ComparisonPropertyName}", DisplayNames.NewPassword.ToLower()))
            .WithName(DisplayNames.ConfirmationPassword);
    }
    #endregion
}