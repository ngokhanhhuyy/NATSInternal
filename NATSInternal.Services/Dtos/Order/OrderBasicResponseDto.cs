namespace NATSInternal.Services.Dtos;

public class OrderBasicResponseDto : IRevuenueBasicResponseDto<OrderAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime PaidDateTime { get; set; }
    public long Amount { get; set; }
    public bool IsLocked { get; set; }  
    public CustomerBasicResponseDto Customer { get; set; }
    public OrderAuthorizationResponseDto Authorization { get; set; }

    public long AmountAfterVat => Amount;

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
        PaidDateTime = order.PaidDateTime;
        Amount = order.ProductAmountBeforeVat;
        IsLocked = order.IsLocked;
        Customer = new CustomerBasicResponseDto(order.Customer);
    }
}