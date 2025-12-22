using JetBrains.Annotations;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class BrandGetListValidator : AbstractListValidator<
    BrandGetListRequestDto,
    BrandGetListRequestDto.FieldToSort>;