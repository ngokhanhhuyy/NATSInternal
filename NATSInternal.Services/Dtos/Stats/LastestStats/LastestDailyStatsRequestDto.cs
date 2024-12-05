namespace NATSInternal.Services.Dtos;

public class LastestDailyStatsRequestDto : IRequestDto
{
    public int DayCount { get; set; } = 7;
    public bool IncludeToday { get; set; } = true;
}