namespace NATSInternal.Core.Interfaces.Dtos;

internal interface ISummaryBasicResponseDto
{
    #region Properties
    long Cost { get; set; }
    long Expenses { get; set; }
    long GrossRevenue { get; set; }
    long NetRevenue { get; set; }
    long NetProfit { get; set; }
    int NewCustomers { get; set; }
    bool IsTemporarilyClosed { get; set; }
    bool IsOfficiallyClosed { get; set; }
    #endregion
}