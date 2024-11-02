namespace NATSInternal.Services.Dtos;

public class CustomerListRequestDto : ICreatorTrackableListRequestDto
{
    public bool OrderByAscending { get; set; } = true;
    public string OrderByField { get; set; } = nameof(OrderByFieldOption.LastName);
    public string SearchByContent { get; set; }
    public int? CreatedUserId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public bool HasRemainingDebtAmountOnly { get; set; }

    public void TransformValues()
    {
        OrderByField = OrderByField?.ToNullIfEmpty();
        SearchByContent = SearchByContent?.ToNullIfEmpty();
    }
}