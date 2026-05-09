using JetBrains.Annotations;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Products;

[UsedImplicitly]
internal class ProductUpdateValidator : AbstractProductUpsertValidator<ProductUpdateRequestDto>
{
    public ProductUpdateValidator()
    {
        RuleForEach(dto => dto.Photos).SetValidator(new PhotoUpsertValidator(), ruleSets: "CreateAndUpdate");
    }
}