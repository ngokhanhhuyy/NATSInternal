namespace NATSInternal.Core.Dtos;

public class ListMonthYearOptionsResponseDto
{
    public List<ListMonthYearResponseDto> Options { get; set; }
    public ListMonthYearResponseDto Default { get; set; }
}