namespace NATSInternal.Services.Dtos;

public class MonthlyStatsDetailResponseDto : IStatsDetailResponseDto
{
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
    public List<DailyStatsBasicResponseDto> DailyStats { get; set; }

    internal MonthlyStatsDetailResponseDto() { }

    internal MonthlyStatsDetailResponseDto(MonthlyStats monthlyStats)
    {
        RetailGrossRevenue = monthlyStats.RetailGrossRevenue;
        TreatmentGrossRevenue = monthlyStats.TreatmentGrossRevenue;
        ConsultantGrossRevenue = monthlyStats.ConsultantGrossRevenue;
        VatCollectedAmount = monthlyStats.VatCollectedAmount;
        DebtIncurredAmount = monthlyStats.DebtIncurredAmount;
        DebtPaidAmount = monthlyStats.DebtPaidAmount;
        ShipmentCost = monthlyStats.ShipmentCost;
        SupplyCost = monthlyStats.SupplyCost;
        UtilitiesExpenses = monthlyStats.UtilitiesExpenses;
        EquipmentExpenses = monthlyStats.EquipmentExpenses;
        OfficeExpense = monthlyStats.OfficeExpense;
        StaffExpense = monthlyStats.StaffExpense;
        Cost = monthlyStats.Cost;
        Expenses = monthlyStats.Expenses;
        GrossRevenue = monthlyStats.GrossRevenue;
        NetRevenue = monthlyStats.NetRevenue;
        DebtIncurredAmount = monthlyStats.DebtAmount;
        GrossProfit = monthlyStats.GrossProfit;
        NetProfit = monthlyStats.NetProfit;
        OperatingProfit = monthlyStats.OperatingProfit;
        NewCustomers = monthlyStats.NewCustomers;
        TemporarilyClosedDateTime = monthlyStats.TemporarilyClosedDateTime;
        OfficiallyClosedDateTime = monthlyStats.OfficiallyClosedDateTime;
        RecordedYear = monthlyStats.RecordedYear;
        RecordedMonth = monthlyStats.RecordedMonth;
        DailyStats = monthlyStats.DailyStats?
            .Select(ds => new DailyStatsBasicResponseDto(ds))
            .ToList();
    }
}