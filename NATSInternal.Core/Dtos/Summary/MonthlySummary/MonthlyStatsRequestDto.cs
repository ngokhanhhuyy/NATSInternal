namespace NATSInternal.Core.Dtos;

public class MonthlyStatsRequestDto : IRequestDto
{
    #region Properties
    public int RecordedMonth { get; set; }
    public int RecordedYear { get; set; }
    #endregion

    #region Methods
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
    #endregion
}