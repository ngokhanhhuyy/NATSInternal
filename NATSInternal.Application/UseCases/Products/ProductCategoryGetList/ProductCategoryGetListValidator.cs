using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class ProductCategoryGetListValidator
    : BaseSortableAndPageableListValidator<
        ProductCategoryGetListRequestDto,
        ProductCategoryGetListRequestDto.FieldToSort
    >
{
    #region Constructors
    public ProductCategoryGetListValidator()
    {
        RuleFor(dto => dto.SearchContent)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchContent);
    }
    #endregion
}