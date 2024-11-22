namespace NATSInternal.Services.Dtos;

public class OrderDetailResponseDto : IExportProductDetailResponseDto<
        OrderItemResponseDto,
        OrderPhotoResponseDto,
        OrderUpdateHistoryResponseDto,
        OrderItemUpdateHistoryDataDto,
        OrderExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public long AmountBeforeVat { get; set; }
    public long VatAmount { get; set; }
    public string Note { get; set; }
    public bool IsLocked { get; set; }
    public List<OrderItemResponseDto> Items { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public List<OrderPhotoResponseDto> Photos { get; set; }
    public OrderExistingAuthorizationResponseDto Authorization { get; set; }
    public List<OrderUpdateHistoryResponseDto> UpdateHistories { get; set; }

    [JsonIgnore]
    public long Amount => AmountBeforeVat + VatAmount;

    public string ThumbnailUrl => Photos?
        .OrderBy(op => op.Id)
        .Select(op => op.Url)
        .FirstOrDefault();

    internal OrderDetailResponseDto(
            Order order,
            OrderExistingAuthorizationResponseDto authorization)
    {
        Id = order.Id;
        StatsDateTime = order.StatsDateTime;
        CreatedDateTime = order.CreatedDateTime;
        AmountBeforeVat = order.AmountBeforeVat;
        VatAmount = order.VatAmount;
        Note = order.Note;
        IsLocked = order.IsLocked;
        Items = order.Items?.Select(i => new OrderItemResponseDto(i)).ToList();
        Customer = new CustomerBasicResponseDto(order.Customer);
        CreatedUser = new UserBasicResponseDto(order.CreatedUser);
        Photos = order.Photos?.Select(p => new OrderPhotoResponseDto(p)).ToList();
        Authorization = authorization;
        UpdateHistories = order.UpdateHistories?
            .Select(uh => new OrderUpdateHistoryResponseDto(uh))
            .ToList();
    }
}