using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class ProductListValidator
    : BaseSortableAndPageableListValidator<ProductGetListRequestDto, ProductGetListRequestDto.FieldToSort>
{
    #region Constructors
    public ProductListValidator()
    {
        RuleFor(dto => dto.SearchContent)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchContent);
    }
    #endregion
}
