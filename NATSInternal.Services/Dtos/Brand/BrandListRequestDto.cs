namespace NATSInternal.Services.Dtos;

public class BrandListRequestDto : IOrderableListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; } = nameof(OrderByFieldOptions.StatsDateTime);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        OrderByField = OrderByField.ToNullIfEmpty();
    }
}
