﻿namespace NATSInternal.Validation.Validators;

public class SupplyItemValidator : Validator<SupplyItemRequestDto>
{
    public SupplyItemValidator()
    {
        RuleFor(dto => dto.Amount)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.SuppliedQuantity)
            .GreaterThan(0)
            .WithName(DisplayNames.SuppliedQuatity);
    }
}
