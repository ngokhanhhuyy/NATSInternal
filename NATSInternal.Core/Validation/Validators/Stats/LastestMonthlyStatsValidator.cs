namespace NATSInternal.Core.Validation.Validators;

internal class LastestMonthlyStatsValidator : Validator<LatestMonthlyStatsRequestDto>
{
    public LastestMonthlyStatsValidator()
    {
        RuleFor(dto => dto.MonthCount)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12)
            .WithName(DisplayNames.MonthCount);
    }
}
