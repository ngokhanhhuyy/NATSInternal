using JetBrains.Annotations;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Products;

[UsedImplicitly]
internal class ProductCreateValidator : AbstractProductUpsertValidator<ProductCreateRequestDto>
{
    public ProductCreateValidator()
    {
        RuleForEach(dto => dto.Photos).SetValidator(new PhotoUpsertValidator(), ruleSets: "Create");
    }
}