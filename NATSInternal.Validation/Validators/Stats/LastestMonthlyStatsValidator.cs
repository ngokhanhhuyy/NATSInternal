namespace NATSInternal.Validation.Validators;

public class LastestMonthlyStatsValidator : Validator<LastestMonthlyStatsRequestDto>
{
    public LastestMonthlyStatsValidator()
    {
        RuleFor(dto => dto.MonthCount)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12)
            .WithName(DisplayNames.MonthCount);
    }
}
