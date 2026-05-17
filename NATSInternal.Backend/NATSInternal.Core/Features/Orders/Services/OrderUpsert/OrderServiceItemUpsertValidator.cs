using FluentValidation;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Orders;

public class OrderServiceItemUpsertValidator : Validator<OrderServiceItemUpsertRequestDto>
{
    #region Constructors
    public OrderServiceItemUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(OrderServiceItemContracts.NameMaxLength)
            .WithName(DisplayNames.Name);

        RuleSet("OfficialCreateOrUpdate", () =>
        {
            RuleFor(dto => dto.AmountBeforeVatPerUnit)
                .GreaterThan(0)
                .WithName(DisplayNames.AmountBeforeVat);
            RuleFor(dto => dto.VatAmountPerUnit)
                .GreaterThanOrEqualTo(0)
                .WithName(DisplayNames.VatAmount);
            RuleFor(dto => dto.Quantity)
                .GreaterThan(0)
                .WithName(DisplayNames.Quantity);
        });
    }
    #endregion
}
