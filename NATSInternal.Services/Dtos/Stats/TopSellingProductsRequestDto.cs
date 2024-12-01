namespace NATSInternal.Services.Dtos;

public class TopSellingProductsRequestDto : IRequestDto
{
    public string RangeType { get; set; } = nameof(StatsRangeType.Date);
    public int RangeCount { get; set; }
    public bool IncludeTodayOrThisMonth { get; set; } = true; 
    public string Creteria { get; set; } = nameof(TopSellingProductCriteria.Amount);
    public int Count { get; set; } = 5;

    public TopSellingProductsRequestDto()
    {
        if (RangeType == nameof(StatsRangeType.Date))
        {
            RangeCount = 10;
        }
        else
        {
            RangeCount = 1;
        }
    }
}