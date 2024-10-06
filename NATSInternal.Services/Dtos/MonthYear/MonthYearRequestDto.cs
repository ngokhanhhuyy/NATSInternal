namespace NATSInternal.Services.Dtos;

public class MonthYearRequestDto : IRequestDto
{
    public int Month { get; set; }
    public int Year { get; set; }

    public void TransformValues()
    {
    }

    public static MonthYearRequestDto CurrentMonthAndYear
    {
        get
        {
            DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
            return new MonthYearRequestDto
            {
                Month = currentDateTime.Month,
                Year = currentDateTime.Year
            };
        }
    }
}
