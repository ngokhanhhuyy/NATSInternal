namespace NATSInternal.Services.Dtos;

public class OrderBasicResponseDto : IRevuenueBasicResponseDto<OrderAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public long AmountBeforeVat { get; set; }
    public bool IsLocked { get; set; }  
    public CustomerBasicResponseDto Customer { get; set; }
    public OrderAuthorizationResponseDto Authorization { get; set; }

    internal OrderBasicResponseDto(Order order)
    {
        MapFromEntity(order);
    }

    internal OrderBasicResponseDto(
            Order order,
            OrderAuthorizationResponseDto authorization)
    {
        MapFromEntity(order);
        Authorization = authorization;
    }

    private void MapFromEntity(Order order)
    {
        Id = order.Id;
        StatsDateTime = order.StatsDateTime;
        AmountBeforeVat = order.AmountBeforeVat + order.VatAmount;
        IsLocked = order.IsLocked;
        Customer = new CustomerBasicResponseDto(order.Customer);
    }
}