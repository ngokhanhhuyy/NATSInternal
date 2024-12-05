﻿namespace NATSInternal.Validation.Validators;

public class TopPurchasedCustomerListValidator : Validator<TopPurchasedCustomerListRequestDto>
{
    public TopPurchasedCustomerListValidator()
    {
        RuleFor(dto => dto.RangeType)
            .IsEnumName(typeof(StatsRangeType))
            .WithName(DisplayNames.RangeType);
        RuleFor(dto => dto.RangeLength)
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.RangeLength);
        RuleFor(dto => dto.Creteria)
            .IsEnumName(typeof(TopPurchasedCustomerCriteria))
            .WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Creteria);
        RuleFor(dto => dto.Count)
            .GreaterThanOrEqualTo(5)
            .LessThanOrEqualTo(15)
            .WithName(DisplayNames.Count);
    }
}