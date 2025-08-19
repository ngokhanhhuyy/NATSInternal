namespace NATSInternal.Core.Validation.Validators;

internal class UserListValidator : Validator<UserListRequestDto>
{
    public UserListValidator(IUserService service)
    {
        RuleFor(dto => dto.SortingByFieldName)
            .IsOneOfFieldOptions(service.GetListSortingOptions().FieldOptions)
            .WithName(DisplayNames.SortingByField);
        RuleFor(dto => dto.SearchContent)
            .MaximumLength(255)
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
}
