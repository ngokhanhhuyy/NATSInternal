namespace NATSInternal.Services.Dtos;

public class TopSoldProductInitialResponseDto
{
    public required StatsRangeTypeOptionListResponseDto RangeTypeOptionList { get; set; }
    public required StatsCriteriaOptionListResponseDto CriteriaOptionList { get; set; }
}