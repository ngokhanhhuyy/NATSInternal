namespace NATSInternal.Core.Validation.Validators;

internal class SupplyListValidator : Validator<SupplyListRequestDto>
{
    public SupplyListValidator(ISupplyService service)
    {
        RuleFor(dto => dto.SortingByFieldName)
            .IsOneOfFieldOptions(service.GetListSortingOptions().FieldOptions)
            .WithName(DisplayNames.SortingByField);
        RuleFor(dto => dto.MonthYear)
            .SetValidator(new ListMonthYearValidator())
            .WithName(DisplayNames.RecordedMonthAndYear);
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.Page);
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(5)
            .LessThan(50)
            .WithName(DisplayNames.ResultsPerPage);
    }
}
