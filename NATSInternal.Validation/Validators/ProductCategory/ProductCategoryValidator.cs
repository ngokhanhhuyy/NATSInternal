namespace NATSInternal.Validation.Validators.ProductCategory;

public class ProductCategoryValidator : Validator<ProductCategoryRequestDto>
{
    public ProductCategoryValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(30)
            .WithName(dto => DisplayNames.Get(nameof(dto.Name)));
    }
}
