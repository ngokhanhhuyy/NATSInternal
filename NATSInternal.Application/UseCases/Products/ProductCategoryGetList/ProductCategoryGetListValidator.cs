using JetBrains.Annotations;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class ProductCategoryGetListValidator : AbstractListValidator<
    ProductCategoryGetListRequestDto,
    ProductCategoryGetListRequestDto.FieldToSort>;