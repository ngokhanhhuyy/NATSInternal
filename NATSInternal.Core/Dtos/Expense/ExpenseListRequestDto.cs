namespace NATSInternal.Core.Dtos;

public class ExpenseListRequestDto : IHasStatsListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByFieldName { get; set; }
    public ListMonthYearRequestDto MonthYear { get; set; }
    public ExpenseCategory? Category { get; set; }
    public int? CreatedUserId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    
    public void TransformValues()
    {
        SortingByFieldName = SortingByFieldName?.ToNullIfEmptyOrWhiteSpace();

        if (CreatedUserId == 0)
        {
            CreatedUserId = null;
        }
    }
}