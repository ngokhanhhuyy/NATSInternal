namespace NATSInternal.Core.Validation.Validators;

internal class ConsultantListValidator : Validator<ConsultantListRequestDto>
{
    public ConsultantListValidator(IConsultantService service)
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
            .LessThanOrEqualTo(15)
            .WithName(DisplayNames.ResultsPerPage);
    }
}