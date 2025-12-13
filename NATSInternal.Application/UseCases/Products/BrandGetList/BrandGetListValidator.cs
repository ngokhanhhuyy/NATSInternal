using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class BrandGetListValidator
    : BaseSortableAndPageableListValidator<BrandGetListRequestDto, BrandGetListRequestDto.FieldToSort>
{
    #region Constructors
    public BrandGetListValidator()
    {
        RuleFor(dto => dto.SearchContent)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchContent);
    }
    #endregion
}