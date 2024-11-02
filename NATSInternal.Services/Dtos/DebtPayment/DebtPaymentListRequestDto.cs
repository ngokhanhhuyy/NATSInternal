namespace NATSInternal.Services.Dtos;

public class DebtPaymentListRequestDto
    :
        IFinancialEngageableListRequestDto,
        ICustomerEngageableListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; } = nameof(OrderByFieldOption.StatsDateTime);
    public int Month { get; set; }
    public int Year { get; set; }
    public bool IgnoreMonthYear { get; set; }
    public int? CustomerId { get; set; }
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

        if (CustomerId == 0)
        {
            CustomerId = null;
        }

        if (!IgnoreMonthYear)
        {
            DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
            Month = Month == 0 ? currentDateTime.Month : Month;
            Year = Year == 0 ? currentDateTime.Year : Year;
        }

        if (CustomerId == 0)
        {
            CustomerId = null;
        }

        if (CreatedUserId == 0)
        {
            CreatedUserId = null;
        }
    }
}
