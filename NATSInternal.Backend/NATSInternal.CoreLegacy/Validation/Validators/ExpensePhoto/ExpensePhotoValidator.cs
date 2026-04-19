namespace NATSInternal.Core.Validation.Validators;

internal class ExpensePhotoValidator : Validator<ExpensePhotoRequestDto>
{
    public ExpensePhotoValidator()
    {
        RuleSet("Create", () =>
        {
            RuleFor(dto => dto.File)
                .NotNull()
                .WithName(DisplayNames.File);
        });

        RuleSet("Update", () =>
        {
            RuleFor(dto => dto.File)
                .NotNull()
                .When(dto => !dto.Id.HasValue)
                .WithName(DisplayNames.File);
        });
    }
}