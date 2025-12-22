using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
internal class ProductGetListValidator : AbstractListValidator<
    ProductGetListRequestDto,
    ProductGetListRequestDto.FieldToSort>;