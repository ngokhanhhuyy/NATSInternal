namespace NATSInternal.Core.Common.Dtos;

public class ListMonthYearOptionsResponseDto
{
    public required List<ListMonthYearResponseDto> Options { get; set; }
    public ListMonthYearResponseDto? Default { get; set; }
}