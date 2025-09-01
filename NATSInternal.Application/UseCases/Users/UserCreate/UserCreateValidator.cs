using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Users;

[UsedImplicitly]
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
            .WithName(DisplayNames.Password);
        RuleFor(dto => dto.ConfirmationPassword)
            .NotEmpty()
            .Must((dto, confirmationPassword) => dto.Password == confirmationPassword)
            .WithMessage(ErrorMessages.MismatchedWith.Replace("{ComparisonPropertyName}", DisplayNames.Password))
            .WithName(DisplayNames.Password);
    }
    #endregion
}
