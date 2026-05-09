using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Products;

[UsedImplicitly]
internal class ProductListValidator : Validator<ProductListRequestDto>
{
    #region Constructors
    public ProductListValidator()
    {
        Include(new SearchableListValidator<ProductListRequestDto, ProductListRequestDto.FieldToSort>());
    }
    #endregion
}