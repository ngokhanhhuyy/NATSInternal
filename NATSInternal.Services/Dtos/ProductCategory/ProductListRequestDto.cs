namespace NATSInternal.Services.Dtos;

public class ProductCategoryListRequestDto : IOrderableListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        OrderByField = OrderByField?.ToNullIfEmpty();
    }
}
