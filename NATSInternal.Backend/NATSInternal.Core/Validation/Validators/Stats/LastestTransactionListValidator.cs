namespace NATSInternal.Core.Validation.Validators;

internal class LastestTransactionsValidator : Validator<LatestTransactionsRequestDto>
{
    public LastestTransactionsValidator()
    {
        RuleFor(dto => dto.Count)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(60)
            .WithName(DisplayNames.Count);
    }
}