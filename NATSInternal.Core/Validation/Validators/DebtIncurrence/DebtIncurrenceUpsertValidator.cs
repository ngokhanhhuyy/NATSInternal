namespace NATSInternal.Core.Validation.Validators;

internal class DebtIncurrenceUpsertValidator : Validator<DebtUpsertRequestDto>
{
    public DebtIncurrenceUpsertValidator()
    {
        RuleFor(dto => dto.Amount)
            .NotEmpty()
            .GreaterThan(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.Note)
            .MaximumLength(255)
            .WithName(DisplayNames.Note);
        RuleFor(dto => dto.StatsDateTime)
            .IsValidStatsDateTime()
            .WithName(DisplayNames.StatsDateTime);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.CustomerId)
                .NotEmpty()
                .GreaterThan(0)
                .WithName(DisplayNames.Customer);
        });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.UpdatedReason)
                .NotEmpty()
                .MaximumLength(255)
                .WithName(DisplayNames.Reason);
        });
    }
}