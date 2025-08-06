namespace NATSInternal.Core.Dtos;

public class OrderBasicResponseDto : IHasStatsBasicResponseDto<OrderExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public long Amount { get; set; }
    public bool IsLocked { get; set; }  
    public CustomerBasicResponseDto Customer { get; set; }
    public OrderExistingAuthorizationResponseDto Authorization { get; set; }

    internal OrderBasicResponseDto(Order order)
    {
        MapFromEntity(order);
    }

    internal OrderBasicResponseDto(
            Order order,
            OrderExistingAuthorizationResponseDto authorization)
    {
        MapFromEntity(order);
        Authorization = authorization;
    }

    private void MapFromEntity(Order order)
    {
        Id = order.Id;
        StatsDateTime = order.StatsDateTime;
        Amount = order.AmountBeforeVat + order.VatAmount;
        IsLocked = order.IsLocked;
        Customer = new CustomerBasicResponseDto(order.Customer);
    }
}