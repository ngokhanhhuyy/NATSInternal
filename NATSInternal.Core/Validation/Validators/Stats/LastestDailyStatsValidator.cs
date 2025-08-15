namespace NATSInternal.Core.Validation.Validators;

internal class LastestDailyStatsValidator : Validator<LatestDailyStatsRequestDto>
{
    public LastestDailyStatsValidator()
    {
        RuleFor(dto => dto.DayCount)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(60)
            .WithName(DisplayNames.DayCount);
    }
}