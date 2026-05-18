using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Metadata;

public class MetadataListMonthYearOptionsResponseDto
{
    public required List<StatsMonthYearResponseDto> Options { get; set; }
    public StatsMonthYearResponseDto? Default { get; set; }
}
