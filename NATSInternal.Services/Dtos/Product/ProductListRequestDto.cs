namespace NATSInternal.Services.Dtos;

public class ProductListRequestDto : IOrderableListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; }
    public string CategoryName { get; set; }
    public int? BrandId { get; set; }
    public string ProductName { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        OrderByField = OrderByField?.ToNullIfEmpty();
        CategoryName = CategoryName?.ToNullIfEmpty();
        ProductName = ProductName?.ToNullIfEmpty();
    }
}
