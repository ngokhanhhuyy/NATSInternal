using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Authentication;

[UsedImplicitly]
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