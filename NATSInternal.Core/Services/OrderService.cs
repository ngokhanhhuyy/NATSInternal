namespace NATSInternal.Core.Services;

internal class OrderService
    : ExportProductAbstractService<
        Order,
        OrderItem,
        Photo,
        OrderUpdateHistory,
        OrderListRequestDto,
        OrderUpsertRequestDto,
        OrderItemRequestDto,
        OrderPhotoRequestDto,
        OrderListResponseDto,
        OrderBasicResponseDto,
        OrderDetailResponseDto,
        OrderItemResponseDto,
        OrderPhotoResponseDto,
        OrderUpdateHistoryResponseDto,
        OrderItemUpdateHistoryDataDto,
        OrderUpdateHistoryDataDto,
        OrderCreatingAuthorizationResponseDto,
        OrderExistingAuthorizationResponseDto>,
    IOrderService
{

    public OrderService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IMultiplePhotosService<Order, Photo> photoService,
            IStatsInternalService statsService)
        : base(context, authorizationService, photoService, statsService)
    {
    }

    /// <inheritdoc />
    public async Task<OrderListResponseDto> GetListAsync(OrderListRequestDto requestDto)
    {
        EntityListDto<Order> entityListDto = await GetListOfEntitiesAsync(requestDto);

        return new OrderListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items?
                .Select(o => new OrderBasicResponseDto(o, GetExistingAuthorization(o)))
                .ToList()
        };
    }

    /// <inheritdoc />
    public async Task<OrderDetailResponseDto> GetDetailAsync(int id)
    {
        Order order = await GetEntityAsync(id);

        return new OrderDetailResponseDto(order, GetExistingAuthorization(order));
    }

    /// <inheritdoc />
    protected override DbSet<Order> GetRepository(DatabaseContext context)
    {
        return context.Orders;
    }

    /// <inheritdoc />
    protected override DbSet<OrderItem> GetItemRepository(DatabaseContext context)
    {
        return context.OrderItems;
    }

    /// <inheritdoc />
    protected override OrderUpdateHistoryDataDto InitializeUpdateHistoryDataDto(Order order)
    {
        return new OrderUpdateHistoryDataDto(order);
    }

    /// <inheritdoc />
    protected override async Task AdjustStatsAsync(
            Order order,
            IStatsInternalService statsService,
            bool isIncrementing)
    {
        DateOnly paidDate = DateOnly.FromDateTime(order.StatsDateTime);
        await statsService.IncrementRetailGrossRevenueAsync(
            isIncrementing ? order.AmountBeforeVat : -order.AmountBeforeVat,
            paidDate);
        await statsService.IncrementVatCollectedAmountAsync(
            isIncrementing ? order.VatAmount : -order.VatAmount,
            paidDate);
    }
}
