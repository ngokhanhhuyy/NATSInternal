using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Users;

internal class UserGetListValidator
    : BaseSortableAndPageableListValidator<UserGetListRequestDto, UserGetListRequestDto.FieldToSort>
{
    #region Constructors
    public UserGetListValidator()
    {
        RuleFor(dto => dto.SearchContent)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchContent);
    }
    #endregion
}
