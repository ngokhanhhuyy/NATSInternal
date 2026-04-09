namespace NATSInternal.Core.Validation.Validators;

internal class OrderItemValidator : Validator<OrderItemRequestDto>
{
    public OrderItemValidator()
    {
        RuleFor(dto => dto.AmountBeforeVatPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.VatAmountPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.DefaultVatPercentage);
        RuleFor(dto => dto.Quantity)
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.Quatity);
    }
}
