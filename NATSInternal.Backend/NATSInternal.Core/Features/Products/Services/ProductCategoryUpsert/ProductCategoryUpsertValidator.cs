using JetBrains.Annotations;
using FluentValidation;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Products;

[UsedImplicitly]
internal class ProductCategoryUpsertValidator : Validator<ProductCategoryUpsertRequestDto>
{
    #region Constructors
    public ProductCategoryUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(ProductCategoryContracts.NameMaxLength)
            .WithName(DisplayNames.Name);

    }
    #endregion
}