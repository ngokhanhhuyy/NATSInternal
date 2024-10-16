namespace NATSInternal.Services.Dtos;

public class ExpenseListRequestDto : IFinancialEngageableListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; } = nameof(OrderByFieldOptions.StatsDateTime);
    public int Month { get; set; }
    public int Year { get; set; }
    public bool IgnoreMonthYear { get; set; }
    public ExpenseCategory? Category { get; set; }
    public int? CreatedUserId { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    
    public void TransformValues()
    {
        OrderByField = OrderByField?.ToNullIfEmpty();

        if (CreatedUserId == 0)
        {
            CreatedUserId = null;
        }

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        if (!IgnoreMonthYear)
        {
            if (Month == 0)
            {
                Month = currentDateTime.Month;
            }

            if (Year == 0)
            {
                Year = currentDateTime.Year;
            }
        }
    }
}