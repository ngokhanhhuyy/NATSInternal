﻿namespace NATSInternal.Validation.Validators;

public class OrderListValidator : Validator<OrderListRequestDto>
{
    public OrderListValidator(IOrderService service)
    {
        RuleFor(dto => dto.SortingByField)
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
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsPerPage);
    }
}
