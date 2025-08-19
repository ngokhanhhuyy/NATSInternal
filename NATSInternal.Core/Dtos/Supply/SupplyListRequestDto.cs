namespace NATSInternal.Core.Dtos;

public class SupplyListRequestDto : IHasProductListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByFieldName { get; set; }
    public ListMonthYearRequestDto MonthYear { get; set; }
    public int? CreatedUserId { get; set; }
    public int? ProductId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        SortingByFieldName = SortingByFieldName?.ToNullIfEmpty();

        if (CreatedUserId == 0)
        {
            CreatedUserId = null;
        }

        if (ProductId == 0)
        {
            ProductId = null;
        }
    }
}