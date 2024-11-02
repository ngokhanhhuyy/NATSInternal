namespace NATSInternal.Validation.Validators;

public class ConsultantListValidator : Validator<ConsultantListRequestDto>
{
    public ConsultantListValidator()
    {
        RuleFor(dto => dto.OrderByField)
            .NotEmpty()
            .IsOneOfFieldOptions(FieldOptions)
            .WithName(DisplayNames.OrderByField);
        RuleFor(dto => dto.Month)
            .IsValidQueryStatsMonth()
            .When(dto => !dto.IgnoreMonthYear)
            .WithName(DisplayNames.Month);
        RuleFor(dto => dto.Year)
            .IsValidQueryStatsYear()
            .When(dto => !dto.IgnoreMonthYear)
            .WithName(DisplayNames.Year);
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.Page);
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(5)
            .LessThanOrEqualTo(15)
            .WithName(DisplayNames.ResultsPerPage);
    }

    private static IEnumerable<OrderByFieldOption> FieldOptions
    {
        get => new List<OrderByFieldOption>
        {
            OrderByFieldOption.StatsDateTime,
            OrderByFieldOption.Amount
        };
    }
}