namespace NATSInternal.Core.Validation.Validators;

internal class CustomerListValidator
    : BaseSortableAndPageableListValidator<CustomerListRequestDto, CustomerListRequestDto.FieldToSort>
{
    #region Constructors
    public CustomerListValidator() : base() { }
    #endregion
}
