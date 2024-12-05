namespace NATSInternal.Validation.Validators;

public class LastestTransactionsValidator : Validator<LastestTransactionsRequestDto>
{
    public LastestTransactionsValidator()
    {
        RuleFor(dto => dto.Count)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(60)
            .WithName(DisplayNames.Count);
    }
}