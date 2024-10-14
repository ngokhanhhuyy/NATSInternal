﻿namespace NATSInternal.Validation.Validators;

public class SupplyUpsertValidator : Validator<SupplyUpsertRequestDto>
{
    public SupplyUpsertValidator()
    {
        RuleFor(dto => dto.StatsDateTime)
            .IsValidStatsDateTime()
            .WithName(DisplayNames.StatsDateTime);
        RuleFor(dto => dto.ShipmentFee)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.ShipmentFee);
        RuleFor(dto => dto.Note)
            .MaximumLength(255)
            .WithName(DisplayNames.Note);
        RuleForEach(dto => dto.Items)
            .SetValidator(new SupplyItemValidator());

        RuleSet("Create", () =>
        {
            RuleForEach(dto => dto.Photos)
                .SetValidator(new SupplyPhotoValidator(), ruleSets: "Create");
        });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.UpdatedReason)
                .MaximumLength(255)
                .WithName(DisplayNames.Reason);
            RuleForEach(dto => dto.Photos)
                .SetValidator(new SupplyPhotoValidator(), ruleSets: "Update");
        });
    }
}
