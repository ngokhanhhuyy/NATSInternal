namespace NATSInternal.Services;

internal class OrderService
    : ProductExportableAbstractService<
        Order,
        OrderItem,
        OrderPhoto,
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
        OrderListAuthorizationResponseDto,
        OrderAuthorizationResponseDto>,
    IOrderService
{
    private readonly IAuthorizationInternalService _authorizationService;

    public OrderService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IMultiplePhotosService<Order, OrderPhoto> photoService,
            IStatsInternalService<Order, OrderUpdateHistory> statsService)
        : base(context, authorizationService, photoService, statsService)
    {
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public async Task<OrderListResponseDto> GetListAsync(OrderListRequestDto requestDto)
    {
        EntityListDto<Order> entityListDto = await GetListOfEntitiesAsync(requestDto);

        return new OrderListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items?
                .Select(o =>
                {
                    OrderAuthorizationResponseDto authorization;
                    authorization = _authorizationService.GetOrderAuthorization(o);
                    return new OrderBasicResponseDto(o, authorization);
                }).ToList(),
            MonthYearOptions = await GenerateMonthYearOptions(),
            Authorization = _authorizationService.GetOrderListAuthorization()
        };
    }

    /// <inheritdoc />
    public async Task<OrderDetailResponseDto> GetDetailAsync(int id)
    {
        Order order = await GetEntityAsync(id);
        OrderAuthorizationResponseDto authorizationResponseDto =_authorizationService
            .GetOrderAuthorization(order);

        return new OrderDetailResponseDto(order, authorizationResponseDto);
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
    protected override bool CanAccessUpdateHistories(IAuthorizationInternalService service)
    {
        return service.CanAccessOrderUpdateHistories();
    }

    /// <inheritdoc />
    protected override bool CanSetStatsDateTime(IAuthorizationInternalService service)
    {
        return service.CanSetOrderStatsDateTime();
    }

    /// <inheritdoc />
    protected override bool CanEdit(Order order, IAuthorizationInternalService service)
    {
        return service.CanEditOrder(order);
    }

    /// <inheritdoc />
    protected override bool CanDelete(Order order, IAuthorizationInternalService service)
    {
        return service.CanDeleteOrder(order);
    }

    /// <inheritdoc />
    protected override async Task AdjustStatsAsync(
            Order order,
            IStatsInternalService<Order, OrderUpdateHistory> statsService,
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
