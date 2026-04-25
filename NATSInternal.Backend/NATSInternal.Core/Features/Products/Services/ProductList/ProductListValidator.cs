using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class ProductGetListValidator : AbstractListValidator<
    ProductListRequestDto,
    ProductListRequestDto.FieldToSort>;