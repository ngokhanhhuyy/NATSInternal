namespace NATSInternal.Services.Dtos;

public class LastestMonthlyStatsRequestDto : IRequestDto
{
    public int MonthCount { get; set; } = 1;
    public bool IncludeThisMonth { get; set; } = true;
}
