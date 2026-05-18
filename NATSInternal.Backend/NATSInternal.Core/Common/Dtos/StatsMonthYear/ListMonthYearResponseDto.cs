namespace NATSInternal.Core.Common.Dtos;

public class StatsMonthYearResponseDto
{
    public int Month { get; set; }
    public int Year { get; set; }

    public StatsMonthYearResponseDto() { }

    public StatsMonthYearResponseDto(int year, int month)
    {
        Month = month;
        Year = year;
    }
}
