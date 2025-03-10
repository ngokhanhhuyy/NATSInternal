﻿namespace NATSInternal.Validation.Validators;

public class SupplyItemValidator : Validator<SupplyItemRequestDto>
{
    public SupplyItemValidator()
    {
        RuleFor(dto => dto.ProductAmountPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.Quantity)
            .GreaterThan(0)
            .WithName(DisplayNames.SuppliedQuatity);
    }
}
