namespace NATSInternal.Core.Dtos;

public class TopSoldProductListRequestDto : IRequestDto
{
    public string RangeType { get; set; } = nameof(StatsRangeType.Date);
    public int RangeLength { get; set; } = 1;
    public bool IncludeTodayOrThisMonth { get; set; } = true; 
    public string Creteria { get; set; } = nameof(TopSoldProductCriteria.Amount);
    public int Count { get; set; } = 5;

    public TopSoldProductListRequestDto()
    {
        if (RangeType == nameof(StatsRangeType.Date))
        {
            RangeLength = 10;
        }
        else
        {
            RangeLength = 1;
        }
    }

    public TopSoldProductListRequestDto TransformValues()
    {
        RangeType = RangeType?.ToNullIfEmpty();
        Creteria = Creteria?.ToNullIfEmpty();

        return this;
    }
}