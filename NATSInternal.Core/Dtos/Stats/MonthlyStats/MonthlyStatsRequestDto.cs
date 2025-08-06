namespace NATSInternal.Core.Dtos;

public class MonthlyStatsRequestDto : IRequestDto
{
    public int RecordedMonth { get; set; }
    public int RecordedYear { get; set; }

    public void TransformValues()
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (RecordedYear == 0)
        {
            RecordedYear = currentDate.Year;
        }

        if (RecordedMonth == 0)
        {
            if (RecordedYear == currentDate.Year)
            {
                RecordedMonth = currentDate.Month;
            }
            else
            {
                RecordedMonth = 12;
            }
        }
    }
}