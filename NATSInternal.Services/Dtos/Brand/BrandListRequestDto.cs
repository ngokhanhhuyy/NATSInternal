namespace NATSInternal.Services.Dtos;

public class BrandListRequestDto : IOrderableListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; }
    public int Page { get; set; }
    public int ResultsPerPage { get; set; }

    public void TransformValues()
    {
        OrderByField = OrderByField.ToNullIfEmpty();
    }
}
