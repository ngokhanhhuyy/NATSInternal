namespace NATSInternal.Core.Dtos;

public class MonthlyStatsDetailResponseDto : IStatsDetailResponseDto
{
    #region Constructors
    internal MonthlyStatsDetailResponseDto() { }

    internal MonthlyStatsDetailResponseDto(MonthlySummary monthlySummary)
    {
        RetailGrossRevenue = monthlySummary.RetailGrossRevenue;
        TreatmentGrossRevenue = monthlySummary.TreatmentGrossRevenue;
        ConsultantGrossRevenue = monthlySummary.ConsultantGrossRevenue;
        VatCollectedAmount = monthlySummary.VatCollectedAmount;
        DebtIncurredAmount = monthlySummary.DebtIncurredAmount;
        DebtPaidAmount = monthlySummary.DebtPaidAmount;
        ShipmentCost = monthlySummary.ShipmentCost;
        SupplyCost = monthlySummary.SupplyCost;
        UtilitiesExpenses = monthlySummary.UtilitiesExpenses;
        EquipmentExpenses = monthlySummary.EquipmentExpenses;
        OfficeExpense = monthlySummary.OfficeExpense;
        StaffExpense = monthlySummary.StaffExpense;
        Cost = monthlySummary.Cost;
        Expenses = monthlySummary.Expenses;
        GrossRevenue = monthlySummary.GrossRevenue;
        NetRevenue = monthlySummary.NetRevenue;
        DebtIncurredAmount = monthlySummary.DebtAmount;
        GrossProfit = monthlySummary.GrossProfit;
        NetProfit = monthlySummary.NetProfit;
        OperatingProfit = monthlySummary.OperatingProfit;
        NewCustomers = monthlySummary.NewCustomerCount;
        TemporarilyClosedDateTime = monthlySummary.TemporarilyClosedDateTime;
        OfficiallyClosedDateTime = monthlySummary.OfficiallyClosedDateTime;
        RecordedYear = monthlySummary.RecordedYear;
        RecordedMonth = monthlySummary.RecordedMonth;

        int daysInMonth = DateTime.DaysInMonth(RecordedYear, RecordedMonth);
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (today.Year == RecordedYear && today.Month == RecordedMonth)
        {
            daysInMonth = today.Day;
        }

        for (int i = 0; i <= daysInMonth; i++)
        {
            DateOnly recordedDate = new DateOnly(RecordedYear, RecordedMonth, i);
            DailySummary? dailySummary = monthlySummary.DailySummaries
                .SingleOrDefault(ds => ds.RecordedDate.Equals(recordedDate));

            if (dailySummary is not null)
            {
                DailySummaries.Add(new DailySummaryBasicResponseDto(dailySummary));
            }
            else
            {
                DailySummaries.Add(new DailySummaryBasicResponseDto(recordedDate));
            }
        }
    }
    #endregion

    #region Properties
    public long RetailGrossRevenue { get; set; }
    public long TreatmentGrossRevenue { get; set; }
    public long ConsultantGrossRevenue { get; set; }
    public long VatCollectedAmount { get; set; }
    public long DebtIncurredAmount { get; set; }
    public long DebtPaidAmount { get; set; }
    public long ShipmentCost { get; set; }
    public long SupplyCost { get; set; }
    public long UtilitiesExpenses { get; set; }
    public long EquipmentExpenses { get; set; }
    public long OfficeExpense { get; set; }
    public long StaffExpense { get; set; }
    public long Cost { get; set; }
    public long Expenses { get; set; }
    public long GrossRevenue { get; set; }
    public long NetRevenue { get; set; }
    public long GrossProfit { get; set; }
    public long NetProfit { get; set; }
    public long OperatingProfit { get; set; }
    public int NewCustomers { get; set; }
    public DateTime? TemporarilyClosedDateTime { get; set; }
    public DateTime? OfficiallyClosedDateTime { get; set; }
    public int RecordedYear { get; set; }
    public int RecordedMonth { get; set; }
    public List<DailySummaryBasicResponseDto> DailySummaries { get; } = new();
    #endregion
}