namespace NATSInternal.Core.Common.Dtos;

public class ListMonthYearResponseDto
{
    public int Month { get; set; }
    public int Year { get; set; }

    public ListMonthYearResponseDto() { }

    public ListMonthYearResponseDto(int year, int month)
    {
        Month = month;
        Year = year;
    }
}
