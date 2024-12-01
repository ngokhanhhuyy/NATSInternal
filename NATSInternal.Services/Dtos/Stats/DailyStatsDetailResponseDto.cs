namespace NATSInternal.Services.Dtos;

public class DailyStatsDetailResponseDto : IStatsDetailResponseDto
{
    public long RetailGrossRevenue { get; set; }
    public long TreatmentGrossRevenue { get; set; }
    public long ConsultantGrossRevenue { get; set; }
    public long VatCollectedAmount { get; set; }
    public long DebtAmount { get; set; }
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
    public DateOnly RecordedDate { get; set; }

    internal DailyStatsDetailResponseDto(DailyStats dailyStats)
    {
        RetailGrossRevenue = dailyStats.RetailGrossRevenue;
        TreatmentGrossRevenue = dailyStats.TreatmentGrossRevenue;
        ConsultantGrossRevenue = dailyStats.ConsultantGrossRevenue;
        VatCollectedAmount = dailyStats.VatCollectedAmount;
        DebtAmount = dailyStats.DebtIncurredAmount;
        DebtPaidAmount = dailyStats.DebtPaidAmount;
        ShipmentCost = dailyStats.ShipmentCost;
        SupplyCost = dailyStats.SupplyCost;
        UtilitiesExpenses = dailyStats.UtilitiesExpenses;
        EquipmentExpenses = dailyStats.EquipmentExpenses;
        OfficeExpense = dailyStats.OfficeExpense;
        StaffExpense = dailyStats.StaffExpense;
        Cost = dailyStats.Cost;
        Expenses = dailyStats.Expenses;
        GrossRevenue = dailyStats.GrossRevenue;
        NetRevenue = dailyStats.NetRevenue;
        DebtAmount = dailyStats.DebtAmount;
        GrossProfit = dailyStats.GrossProfit;
        NetProfit = dailyStats.NetProfit;
        OperatingProfit = dailyStats.OperatingProfit;
        TemporarilyClosedDateTime = dailyStats.TemporarilyClosedDateTime;
        OfficiallyClosedDateTime = dailyStats.OfficiallyClosedDateTime;
        RecordedDate = dailyStats.RecordedDate;
    }
}