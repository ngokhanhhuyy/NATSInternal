using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

[UsedImplicitly]
internal class CustomerListValidator : Validator<CustomerListRequestDto>
{
    #region Constructors
    public CustomerListValidator()
    {
        Include(new SearchableListValidator<CustomerListRequestDto, CustomerListRequestDto.FieldToSort>());
    }
    #endregion
}