namespace NATSInternal.Core.Dtos;

public class OrderUpdateHistoryDataDto
{
    public DateTime StatsDateTime { get; set; }
    public string Note { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItemUpdateHistoryDataDto> Items { get; set; }

    public OrderUpdateHistoryDataDto() { }
    
    internal OrderUpdateHistoryDataDto(Order order)
    {
        StatsDateTime = order.StatsDateTime;
        Note = order.Note;
        CustomerId = order.CustomerId;
        Items = order.Items
            .Select(i => new OrderItemUpdateHistoryDataDto(i))
            .ToList();
    }
}