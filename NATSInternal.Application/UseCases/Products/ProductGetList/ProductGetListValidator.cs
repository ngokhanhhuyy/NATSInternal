using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductGetListValidator
    : BaseSortableAndPageableListValidator<ProductGetListRequestDto, ProductGetListRequestDto.FieldToSort>;
