using FluentValidation;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Orders;

internal class OrderUpsertProductItemValidator : Validator<OrderUpsertProductItemRequestDto>
{
    #region Constructors
    public OrderUpsertProductItemValidator()
    {
        RuleFor(dto => dto.ProductId)
            .GreaterThan(0)
            .WithName(DisplayNames.ProductId);

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
