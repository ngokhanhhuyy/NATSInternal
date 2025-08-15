namespace NATSInternal.Core.Validation.Validators.Treatment;

internal class TreatmentUpsertValidator : Validator<TreatmentUpsertRequestDto>
{
    public TreatmentUpsertValidator()
    {
        RuleFor(dto => dto.StatsDateTime)
            .IsValidStatsDateTime()
            .WithName(DisplayNames.StatsDateTime);
        RuleFor(dto => dto.ServiceAmountBeforeVat)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.ServiceAmount);
        RuleFor(dto => dto.ServiceVatAmount)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.VatAmount);
        RuleFor(dto => dto.Note)
            .MaximumLength(255)
            .WithName(DisplayNames.Note);
        RuleFor(dto => dto.CustomerId)
            .NotEmpty()
            .WithName(DisplayNames.Customer);
        RuleFor(dto => dto.UpdatedReason)
            .MaximumLength(255)
            .WithName(DisplayNames.Reason);
        RuleFor(dto => dto.Items)
            .NotEmpty()
            .WithName(DisplayNames.TreatmentItem);
        RuleForEach(dto => dto.Items)
            .NotNull()
            .SetValidator(new TreatmentItemValidator())
            .WithName(DisplayNames.TreatmentItem);
        RuleForEach(dto => dto.Photos)
            .NotNull()
            .SetValidator(new TreatmentPhotoValidator());

        RuleSet("Create", () => { });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.UpdatedReason)
                .NotEmpty()
                .MaximumLength(255)
                .WithName(DisplayNames.Reason);
        });
    }
}
