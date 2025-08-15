namespace NATSInternal.Validation.Validators;

public class OrderUpsertValidator : Validator<OrderUpsertRequestDto>
{
    public OrderUpsertValidator()
    {
        RuleFor(dto => dto.StatsDateTime)
            .IsValidStatsDateTime()
            .WithName(DisplayNames.StatsDateTime);
        RuleFor(dto => dto.Note)
            .MaximumLength(255)
            .WithName(DisplayNames.Note);
        RuleFor(dto => dto.CustomerId)
            .NotEmpty()
            .WithName(DisplayNames.Customer);
        RuleFor(dto => dto.Items)
            .NotEmpty()
            .WithName(DisplayNames.Product);
        RuleForEach(dto => dto.Items)
            .SetValidator(new OrderItemValidator());

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
