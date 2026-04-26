using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Products;

[UsedImplicitly]
internal class ProductListValidator : AbstractListValidator<
    ProductListRequestDto,
    ProductListRequestDto.FieldToSort>;