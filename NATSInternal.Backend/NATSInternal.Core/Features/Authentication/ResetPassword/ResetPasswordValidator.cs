using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Authentication;

[UsedImplicitly]
internal class UserResetPasswordValidator : Validator<ResetPasswordRequestDto>
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