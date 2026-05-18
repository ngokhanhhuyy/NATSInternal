using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Metadata;

public class MetadataListMonthYearOptionsResponseDto
{
    public required List<ListMonthYearResponseDto> Options { get; set; }
    public ListMonthYearResponseDto? Default { get; set; }
}
