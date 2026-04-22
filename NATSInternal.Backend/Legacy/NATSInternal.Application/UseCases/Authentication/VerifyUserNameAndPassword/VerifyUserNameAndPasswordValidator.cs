using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Authentication;

internal class VerifyUserNameAndPasswordValidator : Validator<VerifyUserNameAndPasswordRequestDto>
{
    #region Constructors
    public VerifyUserNameAndPasswordValidator()
    {
        RuleFor(dto => dto.UserName)
            .NotEmpty()
            .WithName(DisplayNames.UserName);
        RuleFor(dto => dto.Password)
            .NotEmpty()
            .WithName(DisplayNames.Password);
    }
    #endregion
}