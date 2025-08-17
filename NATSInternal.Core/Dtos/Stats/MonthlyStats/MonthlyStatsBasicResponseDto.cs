namespace NATSInternal.Core.Dtos;

public class MonthlyStatsBasicResponseDto : IStatsBasicResponseDto
{
    public long Cost { get; set; }
    public long Expenses { get; set; }
    public long GrossRevenue { get; set; }
    public long NetRevenue { get; set; }
    public long NetProfit { get; set; }
    public int NewCustomers { get; set; }
    public bool IsTemporarilyClosed { get; set; }
    public bool IsOfficiallyClosed { get; set; }
    public int RecordedYear { get; set; }
    public int RecordedMonth { get; set; }

    internal MonthlyStatsBasicResponseDto(MonthlySummary stats)
    {
        Cost = stats.Cost;
        Expenses = stats.Expenses;
        GrossRevenue = stats.GrossRevenue;
        NetRevenue = stats.NetRevenue;
        NetProfit = stats.NetProfit;
        NewCustomers = stats.NewCustomerCount;
        IsTemporarilyClosed = stats.TemporarilyClosedDateTime.HasValue;
        IsOfficiallyClosed = stats.OfficiallyClosedDateTime.HasValue;
        RecordedYear = stats.RecordedYear;
        RecordedMonth = stats.RecordedMonth;
    }
}