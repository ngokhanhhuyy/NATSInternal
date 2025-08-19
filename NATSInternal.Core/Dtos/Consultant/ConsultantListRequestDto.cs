namespace NATSInternal.Core.Dtos;

public class ConsultantListRequestDto
    :
        IHasStatsListRequestDto,
        IHasCustomerListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByFieldName { get; set; }
    public ListMonthYearRequestDto MonthYear { get; set; }
    public int? CustomerId { get; set; }
    public int? CreatedUserId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        SortingByFieldName = SortingByFieldName?.ToNullIfEmpty();

        if (CreatedUserId == 0)
        {
            CreatedUserId = null;
        }
    }
}