namespace NATSInternal.Services.Dtos;

public class MonthlyStatsRequestDto : IRequestDto
{
    public int RecordedMonth { get; set; }
    public int RecordedYear { get; set; }

    public void TransformValues()
    {
    }
}