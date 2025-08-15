namespace NATSInternal.Core.Validation.Validators;

internal class CustomerListValidator : Validator<CustomerListRequestDto>
{
    public CustomerListValidator(ICustomerService service)
    {
        RuleFor(dto => dto.SortingByField)
            .IsOneOfFieldOptions(service.GetListSortingOptions().FieldOptions)
            .WithName(DisplayNames.SortingByField);
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(1)
            .WithName(dto => DisplayNames.Get(nameof(dto.Page)));
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(10)
            .LessThanOrEqualTo(50)
            .WithName(dto => DisplayNames.Get(nameof(dto.ResultsPerPage)));
        RuleFor(dto => dto.SearchByContent)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchByContent);
    }
}
