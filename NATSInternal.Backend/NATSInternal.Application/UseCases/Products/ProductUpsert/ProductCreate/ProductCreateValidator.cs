using JetBrains.Annotations;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class ProductCreateValidator : AbstractProductUpsertValidator<ProductCreateRequestDto>
{
    public ProductCreateValidator()
    {
        RuleForEach(dto => dto.Photos).SetValidator(new PhotoAddOrUpdateValidator(), ruleSets: "Create");
    }
}