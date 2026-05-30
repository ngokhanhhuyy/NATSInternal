using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Customers;

namespace NATSInternal.Core.Features.Orders;

[UsedImplicitly]
internal class OrderUpsertCustomerValidator : Validator<OrderUpsertCustomerRequestDto>
{
    #region Constructors
    public OrderUpsertCustomerValidator(IValidator<CustomerUpsertRequestDto> customerValidator)
    {
        RuleFor(dto => dto.Id)
            .NotEmpty()
            .When(dto => !dto.CreateNewCustomer)
            .WithName(DisplayNames.Customer);

        #nullable disable
        RuleFor(dto => dto.Create)
            .SetValidator(customerValidator)
            .When(dto => dto.CreateNewCustomer)
            .WithName(DisplayNames.Customer);
        #nullable enable
    }
    #endregion
}
