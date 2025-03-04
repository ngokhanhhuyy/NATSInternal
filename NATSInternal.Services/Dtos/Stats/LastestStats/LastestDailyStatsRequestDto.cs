namespace NATSInternal.Services.Dtos;

public class LatestDailyStatsRequestDto : IRequestDto
{
    public int DayCount { get; set; } = 7;
    public bool IncludeToday { get; set; } = true;
}