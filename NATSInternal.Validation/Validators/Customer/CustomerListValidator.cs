namespace NATSInternal.Validation.Validators;

public class CustomerListValidator : Validator<CustomerListRequestDto>
{
    public CustomerListValidator()
    {
        RuleFor(dto => dto.OrderByField)
            .NotEmpty()
            .IsOneOfFieldOptions(FieldOptions)
            .WithName(dto => DisplayNames.Get(nameof(dto.OrderByField)));
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(1)
            .WithName(dto => DisplayNames.Get(nameof(dto.Page)));
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(10)
            .LessThanOrEqualTo(50)
            .WithName(dto => DisplayNames.Get(nameof(dto.ResultsPerPage)));
    }

    private static IEnumerable<OrderByFieldOptions> FieldOptions
    {
        get => new List<OrderByFieldOptions>
        {
            OrderByFieldOptions.LastName,
            OrderByFieldOptions.FullName,
            OrderByFieldOptions.Birthday,
            OrderByFieldOptions.CreatedDateTime,
            OrderByFieldOptions.DebtRemainingAmount
        };
    }
}
