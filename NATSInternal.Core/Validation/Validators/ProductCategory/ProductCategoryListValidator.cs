namespace NATSInternal.Core.Validation.Validators;

internal class ProductCategoryListValidator : Validator<ProductCategoryListRequestDto>
{
    public ProductCategoryListValidator(IProductCategoryService service)
    {
        RuleFor(dto => dto.SortingByField)
            .IsOneOfFieldOptions(service.GetListSortingOptions().FieldOptions)
            .WithName(DisplayNames.SortingByField);
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(1)
            .WithName(dto => DisplayNames.Get(nameof(dto.Page)));
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(10)
            .LessThanOrEqualTo(50)
            .WithName(dto => DisplayNames.Get(nameof(dto.ResultsPerPage)));
    }
}
