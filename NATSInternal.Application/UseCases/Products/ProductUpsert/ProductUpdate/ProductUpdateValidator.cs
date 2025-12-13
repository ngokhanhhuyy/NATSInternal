using JetBrains.Annotations;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class ProductUpdateValidator : AbstractProductUpsertValidator<ProductUpdateRequestDto>
{
    public ProductUpdateValidator()
    {
        RuleForEach(dto => dto.Photos).SetValidator(new PhotoAddOrUpdateValidator(), ruleSets: "CreateAndUpdate");
    }
}