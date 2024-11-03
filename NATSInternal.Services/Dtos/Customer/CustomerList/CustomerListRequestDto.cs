namespace NATSInternal.Services.Dtos;

public class CustomerListRequestDto : ICreatorTrackableListRequestDto
{
    public bool? SortingByAscending { get; set; } = true;
    public string SortingByField { get; set; }
    public string SearchByContent { get; set; }
    public int? CreatedUserId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public bool HasRemainingDebtAmountOnly { get; set; }

    public void TransformValues()
    {
        SortingByField = SortingByField?.ToNullIfEmpty();
        SearchByContent = SearchByContent?.ToNullIfEmpty();
    }
}