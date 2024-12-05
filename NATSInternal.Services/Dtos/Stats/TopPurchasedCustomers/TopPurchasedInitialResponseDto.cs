namespace NATSInternal.Services.Dtos;

public class TopPurchasedInitialResponseDto
{
    public required StatsRangeTypeOptionListResponseDto RangeTypeOptionList { get; set; }
    public required StatsCriteriaOptionListResponseDto CriteriaOptionList { get; set; }
}