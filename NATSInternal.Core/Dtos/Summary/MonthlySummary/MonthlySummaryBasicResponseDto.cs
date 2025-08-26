namespace NATSInternal.Core.Dtos;

public class MonthlySummaryBasicResponseDto : ISummaryBasicResponseDto
{
    #region Constructors
    internal MonthlySummaryBasicResponseDto(int recordedYear, int recordedMonth)
    {
        RecordedYear = recordedYear;
        RecordedMonth = recordedMonth;
    }

    internal MonthlySummaryBasicResponseDto(DateOnly recordedDate)
    {
        RecordedYear = recordedDate.Year;
        RecordedMonth = recordedDate.Month;
    }

    internal MonthlySummaryBasicResponseDto(MonthlySummary stats)
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
    #endregion

    #region Properties
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
    #endregion
}