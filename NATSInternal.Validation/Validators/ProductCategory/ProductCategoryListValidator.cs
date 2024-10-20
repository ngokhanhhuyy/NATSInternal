namespace NATSInternal.Validation.Validators;

public class ProductCategoryListValidator : Validator<ProductCategoryListRequestDto>
{
    public ProductCategoryListValidator()
    {
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(1)
            .WithName(dto => DisplayNames.Get(nameof(dto.Page)));
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(10)
            .LessThanOrEqualTo(50)
            .WithName(dto => DisplayNames.Get(nameof(dto.ResultsPerPage)));
    }
}
