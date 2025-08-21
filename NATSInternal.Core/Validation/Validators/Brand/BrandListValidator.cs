namespace NATSInternal.Core.Validation.Validators;

internal class BrandListValidator
    : BaseSortableAndPageableListValidator<BrandListRequestDto, BrandListRequestDto.FieldToSort>
{
    #region Constructors
    public BrandListValidator() : base() { }
    #endregion
}
