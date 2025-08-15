namespace NATSInternal.Core.Validation.Validators.ProductCategory;

internal class ProductCategoryUpsertValidator : Validator<ProductCategoryRequestDto>
{
    public ProductCategoryUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(30)
            .WithName(dto => DisplayNames.Get(nameof(dto.Name)));
    }
}
