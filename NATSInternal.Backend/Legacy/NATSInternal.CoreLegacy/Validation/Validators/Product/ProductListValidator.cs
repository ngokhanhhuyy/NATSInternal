namespace NATSInternal.Core.Validation.Validators;

internal class ProductListValidator
        : BaseSortableAndPageableListValidator<ProductListRequestDto, ProductListRequestDto.FieldToSort>
{
    #region Constructors
    public ProductListValidator() { }
    #endregion
}
