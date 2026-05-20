using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Metadata;

public class MetadataStatsMonthYearSeries
{
    #region Properties
    public required List<StatsMonthYearResponseDto> OrderSeries { get; init; }
    #endregion
}
