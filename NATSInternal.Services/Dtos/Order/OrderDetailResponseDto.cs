namespace NATSInternal.Services.Dtos;

public class OrderDetailResponseDto : IProductExportableDetailResponseDto<
        OrderItemResponseDto,
        OrderPhotoResponseDto,
        OrderUpdateHistoryResponseDto,
        OrderAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public long AmountBeforeVat { get; set; }
    public long VatAmount { get; set; }
    public long Amount { get; set; }
    public string Note { get; set; }
    public bool IsLocked { get; set; }
    public List<OrderItemResponseDto> Items { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public List<OrderPhotoResponseDto> Photos { get; set; }
    public OrderAuthorizationResponseDto Authorization { get; set; }
    public List<OrderUpdateHistoryResponseDto> UpdateHistories { get; set; }

    public string ThumbnailUrl => Photos?
        .OrderBy(op => op.Id)
        .Select(op => op.Url)
        .FirstOrDefault();

    internal OrderDetailResponseDto(Order order, OrderAuthorizationResponseDto authorization)
    {
        Id = order.Id;
        StatsDateTime = order.StatsDateTime;
        CreatedDateTime = order.CreatedDateTime;
        AmountBeforeVat = order.ProductAmountBeforeVat;
        VatAmount = order.ProductVatAmount;
        Note = order.Note;
        IsLocked = order.IsLocked;
        Items = order.Items?.Select(i => new OrderItemResponseDto(i)).ToList();
        Customer = new CustomerBasicResponseDto(order.Customer);
        CreatedUser = new UserBasicResponseDto(order.CreatedUser);
        Photos = order.Photos?.Select(p => new OrderPhotoResponseDto(p)).ToList();
        Authorization = authorization;
        UpdateHistories = order.UpdateHistories
            .Select(uh => new OrderUpdateHistoryResponseDto(uh))
            .ToList();
    }
}