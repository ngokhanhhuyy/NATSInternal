namespace NATSInternal.Validation.Validators;

public class UserListValidator : Validator<UserListRequestDto>
{
    public UserListValidator()
    {
        RuleFor(dto => dto.OrderByField)
            .NotNull()
            .IsOneOfFieldOptions(FieldOptions)
            .WithName(dto => DisplayNames.Get(nameof(dto.OrderByField)));
        RuleFor(dto => dto.Content)
            .MinimumLength(3)
            .MaximumLength(255)
            .When(dto => dto.Content != null && dto.Content.Length != 0)
            .WithName(DisplayNames.Content);
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(Rules.PageMinimumValue)
            .WithName(dto => DisplayNames.Get(nameof(dto.Page)));
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(Rules.ResultsPerPageMinimumValue)
            .LessThanOrEqualTo(Rules.ResultsPerPageMaximumValue)
            .WithName(dto => DisplayNames.Get(nameof(dto.ResultsPerPage)));
    }

    public static class Rules
    {
        public const int PageMinimumValue = 1;
        public const int ResultsPerPageMinimumValue = 5;
        public const int ResultsPerPageMaximumValue = 50;
    }

    private static IEnumerable<OrderByFieldOption> FieldOptions
    {
        get => new List<OrderByFieldOption>
        {
            OrderByFieldOption.LastName,
            OrderByFieldOption.FirstName,
            OrderByFieldOption.UserName,
            OrderByFieldOption.Birthday,
            OrderByFieldOption.Age,
            OrderByFieldOption.CreatedDateTime,
            OrderByFieldOption.Role,
        };
    }
}
