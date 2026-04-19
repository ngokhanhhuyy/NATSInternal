namespace NATSInternal.Core.Validation.Validators.Product;

internal class ProductUpsertValidator : Validator<ProductUpsertRequestDto>
{
    #region Constructors
    public ProductUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(50)
            .WithName(DisplayNames.Name);
        RuleFor(dto => dto.Description)
            .MaximumLength(1000)
            .WithName(DisplayNames.Description);
        RuleFor(dto => dto.Unit)
            .NotEmpty()
            .MaximumLength(12)
            .WithName(DisplayNames.Unit);
        RuleFor(dto => dto.DefaultAmountBeforeVatPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.DefaultPrice);
        RuleFor(dto => dto.DefaultVatPercentage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .WithName(DisplayNames.DefaultVatPercentage);
        RuleFor(dto => dto.Photos).ContainsNoOrOneThumbnail();

        RuleSet("Create", () =>
        {
            RuleForEach(dto => dto.Photos)
                .SetValidator(new PhotoValidator(), ruleSets: "Create");
        });

        RuleSet("CreateAndUpdate", () =>
        {
            RuleForEach(dto => dto.Photos)
                .SetValidator(new PhotoValidator(), ruleSets: "CreateAndUpdate");
        });
    }
    #endregion
}
