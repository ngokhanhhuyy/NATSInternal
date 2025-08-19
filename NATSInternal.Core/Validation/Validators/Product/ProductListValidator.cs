namespace NATSInternal.Core.Validation.Validators;

internal class ProductListValidator : Validator<ProductListRequestDto>
{
    public ProductListValidator(IProductService service)
    {
        RuleFor(dto => dto.SortingByFieldName)
            .IsOneOfFieldOptions(service.GetListSortingOptions().FieldOptions)
            .WithName(DisplayNames.SortingByField);
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.Page);
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(5)
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsPerPage);
        RuleFor(dto => dto.ProductName)
            .MaximumLength(255)
            .WithName(DisplayNames.Product);
    }
}
