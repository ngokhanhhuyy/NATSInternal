using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Users;

[UsedImplicitly]
internal class UserUpdateValidator : Validator<UserUpdateRequestDto>
{
    #region Constructors
    public UserUpdateValidator()
    {
        RuleFor(dto => dto.RoleNames)
            .NotEmpty()
            .WithName(DisplayNames.Role);
    }
    #endregion
}