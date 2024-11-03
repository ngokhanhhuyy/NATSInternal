namespace NATSInternal.Validation.Validators.ProductCategory;

public class ProductCategoryUpsertValidator : Validator<ProductCategoryRequestDto>
{
    public ProductCategoryUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(30)
            .WithName(dto => DisplayNames.Get(nameof(dto.Name)));
    }
}
