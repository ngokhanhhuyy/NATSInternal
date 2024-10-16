namespace NATSInternal.Validation.Validators;

public class ConsultantUpsertValidator : Validator<ConsultantUpsertRequestDto>
{
    public ConsultantUpsertValidator()
    {
        RuleFor(dto => dto.StatsDateTime)
            .IsValidStatsDateTime()
            .WithName(DisplayNames.StatsDateTime);
        RuleFor(dto => dto.AmountBeforeVat)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.AmountBeforeVat);
        RuleFor(dto => dto.VatAmount)
            .GreaterThan(0)
            .WithName(DisplayNames.VatAmount);
        RuleFor(dto => dto.Note)
            .MaximumLength(255)
            .WithName(DisplayNames.Note);
        RuleFor(dto => dto.CustomerId)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.Customer);

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