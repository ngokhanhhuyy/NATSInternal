namespace NATSInternal.Core.Dtos;

public class LatestMonthlyStatsRequestDto : IRequestDto
{
    public int MonthCount { get; set; } = 1;
    public bool IncludeThisMonth { get; set; } = true;
}
