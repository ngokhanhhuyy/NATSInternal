using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Users;

[UsedImplicitly]
public class UserAddToOrRemoveFromRolesValidator<TRequestDto> : Validator<TRequestDto>
    where TRequestDto : UserAddToOrRemoveFromRolesRequestDto
{
    public UserAddToOrRemoveFromRolesValidator()
    {
        RuleFor(dto => dto.RoleNames)
            .NotEmpty()
            .WithName(DisplayNames.Role);
    }
}