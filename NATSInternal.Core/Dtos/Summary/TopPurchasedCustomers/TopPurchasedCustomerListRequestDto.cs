namespace NATSInternal.Core.Dtos;

public class TopPurchasedCustomerListRequestDto : IRequestDto
{
    public string RangeType { get; set; } = nameof(StatsRangeType.Date);
    public int RangeLength { get; set; } = 1;
    public bool IncludeTodayOrThisMonth { get; set; } = true;
    public string Creteria { get; set; }
    public int Count { get; set; } = 5;

    public TopPurchasedCustomerListRequestDto()
    {
        if (RangeType == nameof(StatsRangeType.Date))
        {
            RangeLength = 10;
        }
        else
        {
            RangeLength = 1;
        }

        Creteria = nameof(TopPurchasedCustomerCriteria.PurchasedAmount);
    }

    public TopPurchasedCustomerListRequestDto TransformValues()
    {
        RangeType = RangeType?.ToNullIfEmptyOrWhiteSpace();
        Creteria = Creteria?.ToNullIfEmptyOrWhiteSpace();

        return this;
    }
}
