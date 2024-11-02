namespace NATSInternal.Validation.Validators;

public class ExpenseListValidator : Validator<ExpenseListRequestDto>
{
    public ExpenseListValidator()
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
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsPerPage);
    }

    private static IEnumerable<OrderByFieldOption> FieldOptions
    {
        get => new List<OrderByFieldOption>
        {
            OrderByFieldOption.Amount,
            OrderByFieldOption.StatsDateTime
        };
    }
}