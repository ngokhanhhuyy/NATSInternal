namespace NATSInternal.Core.Validation;

internal class ExpenseUpsertValidator : Validator<ExpenseUpsertRequestDto>
{
    public ExpenseUpsertValidator()
    {
        RuleFor(dto => dto.StatsDateTime)
            .IsValidStatsDateTime()
            .WithName(DisplayNames.StatsDateTime);
        RuleFor(dto => dto.Amount)
            .GreaterThan(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.Category)
            .IsInEnum().WithMessage(ErrorMessages.Invalid)
            .WithName(DisplayNames.Category);
        RuleFor(dto => dto.Note)
            .MaximumLength(255)
            .WithName(DisplayNames.Note);
        RuleFor(dto => dto.PayeeName)
            .NotEmpty()
            .MaximumLength(100)
            .WithName(DisplayNames.PayeeName);

        RuleSet("Create", () =>
        {
            RuleForEach(dto => dto.Photos)
                .SetValidator(new ExpensePhotoValidator(), ruleSets: "Create");
        });

        RuleSet("Update", () =>
        {
            RuleForEach(dto => dto.Photos)
                .SetValidator(new ExpensePhotoValidator(), ruleSets: "Update");
            RuleFor(dto => dto.UpdatedReason)
                .NotEmpty()
                .MaximumLength(255)
                .WithName(DisplayNames.UpdatedReason);
        });
    }
}
