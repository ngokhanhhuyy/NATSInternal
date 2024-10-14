namespace NATSInternal.Validation.Validators;

public class TreatmentItemValidator : Validator<TreatmentItemRequestDto>
{
    public TreatmentItemValidator()
    {
        RuleFor(dto => dto.ProductAmountPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.VatAmountPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.VatAmount);
        RuleFor(dto => dto.Quantity)
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.Quatity);
        RuleFor(dto => dto.ProductId)
            .NotEmpty()
            .WithName(DisplayNames.Product);
    }
}
