namespace NATSInternal.Services.Dtos;

public class ProductListRequestDto : IOrderableListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByField { get; set; }
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public string ProductName { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        SortingByField = SortingByField?.ToNullIfEmpty();
        ProductName = ProductName?.ToNullIfEmpty();
    }
}
