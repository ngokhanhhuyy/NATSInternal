﻿namespace NATSInternal.Validation.Validators;

public class ListMonthYearValidator : Validator<ListMonthYearRequestDto>
{
    public ListMonthYearValidator()
    {
        RuleFor(dto => dto.Month)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(DateTime.UtcNow.ToApplicationTime().Month)
            .When(dto => dto.Year == DateTime.UtcNow.ToApplicationTime().Year)
            .LessThanOrEqualTo(12)
            .When(dto => dto.Year < DateTime.UtcNow.ToApplicationTime().Year)
            .WithName(DisplayNames.Month);
        RuleFor(dto => dto.Year)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(DateTime.UtcNow.ToApplicationTime().Year)
            .WithName(DisplayNames.Year);
    }
}
