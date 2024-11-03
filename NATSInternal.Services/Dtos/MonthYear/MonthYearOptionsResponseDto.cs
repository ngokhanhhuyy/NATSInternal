namespace NATSInternal.Services.Dtos;

public class MonthYearOptionsResponseDto
{
    public List<MonthYearResponseDto> Items { get; set; }
    public MonthYearResponseDto Default { get; set; }
}