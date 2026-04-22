namespace NATSInternal.Core.Validation.Validators.ProductCategory;

internal class ProductCategoryUpsertValidator : Validator<ProductCategoryUpsertRequestDto>
{
    #region Constructors
    public ProductCategoryUpsertValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(ProductCategoryContracts.NameMaxLength)
            .WithName(dto => DisplayNames.Name);
    }
    #endregion
}
