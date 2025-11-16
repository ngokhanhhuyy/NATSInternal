using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Customers;

[UsedImplicitly]
internal class CustomerGetListValidator
    : BaseSortableAndPageableListValidator<CustomerGetListRequestDto, CustomerGetListRequestDto.FieldToSort>
{
    #region Constructors
    public CustomerGetListValidator()
    {
        RuleFor(dto => dto.SearchContent)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchContent);
    }
    #endregion
}