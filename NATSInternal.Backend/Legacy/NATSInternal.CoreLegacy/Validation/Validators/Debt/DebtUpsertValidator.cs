namespace NATSInternal.Core.Validation.Validators;

internal class DebtUpsertValidator : Validator<DebtUpsertRequestDto>
{
    #region Constructors
    public DebtUpsertValidator()
    {
        RuleFor(dto => dto.Amount)
            .NotEmpty()
            .GreaterThan(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.Note)
            .MaximumLength(HasStatsContracts.NoteMaxLength)
            .WithName(DisplayNames.Note);
        RuleFor(dto => dto.StatsDateTime)
            .IsValidStatsDateTime()
            .WithName(DisplayNames.StatsDateTime);

        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.CustomerId)
                .NotEmpty()
                .WithName(DisplayNames.Customer);
        });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.UpdatedReason)
                .NotEmpty()
                .MaximumLength(HasStatsContracts.UpdatedReasonMaxLength)
                .WithName(DisplayNames.UpdatedReason);
        });
    }
    #endregion
}