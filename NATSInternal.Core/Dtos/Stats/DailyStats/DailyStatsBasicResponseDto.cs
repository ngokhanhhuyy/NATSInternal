namespace NATSInternal.Core.Dtos;

public class DailyStatsBasicResponseDto : IStatsBasicResponseDto
{
    public long Cost { get; set; }
    public long Expenses { get; set; }
    public long GrossRevenue { get; set; }
    public long NetRevenue { get; set; }
    public long NetProfit { get; set; }
    public int NewCustomers { get; set; }
    public bool IsTemporarilyClosed { get; set; }
    public bool IsOfficiallyClosed { get; set; }
    public DateOnly RecordedDate { get; set; }

    internal DailyStatsBasicResponseDto(DateOnly recordedDate)
    {
        RecordedDate = recordedDate;
    }

    internal DailyStatsBasicResponseDto(DailySummary stats)
    {
        Cost = stats.Cost;
        Expenses = stats.Expenses;
        GrossRevenue = stats.GrossRevenue;
        NetRevenue = stats.NetRevenue;
        NetProfit = stats.NetProfit;
        NewCustomers = stats.NewCustomerCount;
        IsTemporarilyClosed = stats.TemporarilyClosedDateTime.HasValue;
        IsOfficiallyClosed = stats.OfficiallyClosedDateTime.HasValue;
        RecordedDate = stats.RecordedDate;
    }
}