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
            .WithName(dto => DisplayNames.UserName);
        RuleFor(dto => dto.Password)
            .NotEmpty()
            .WithName(dto => DisplayNames.Password);
    }
    #endregion
}