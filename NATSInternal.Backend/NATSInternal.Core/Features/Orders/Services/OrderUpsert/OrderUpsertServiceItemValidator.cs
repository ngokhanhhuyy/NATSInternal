using FluentValidation;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Orders;

public class OrderUpsertServiceItemValidator : Validator<OrderUpsertServiceItemRequestDto>
{
    #region Constructors
    public OrderUpsertServiceItemValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(OrderServiceItemContracts.NameMaxLength)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.AmountBeforeVatPerUnit)
            .GreaterThan(0)
            .WithName(DisplayNames.AmountBeforeVat);
        RuleFor(dto => dto.VatAmountPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.VatAmount);
        RuleFor(dto => dto.Quantity)
            .GreaterThan(0)
            .WithName(DisplayNames.Quantity);
    }
    #endregion
}