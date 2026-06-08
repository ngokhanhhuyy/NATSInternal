using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Customers;

namespace NATSInternal.Core.Features.Orders;

public class OrderBasicResponseDto
{
    #region Constructors
    internal OrderBasicResponseDto(Order order)
    {
        Id = order.Id;
        Type = order.Type;
        StatsDate = order.StatsDate;
        AmountAfterVat = order.CachedAmountAfterVat;
        IsDebtOrder = order.EffectivePayment is null || order.CachedAmountAfterVat > order.EffectivePayment.Amount;
        Customer = new(order.Customer);
        ThumbnailUrl = order.ThumbnailUrl;
    }

    internal OrderBasicResponseDto(Order order, OrderExistingAuthorizationResponseDto authorization) : this(order)
    {
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public int Id { get; set; }
    public OrderType Type { get; set; }
    public DateOnly StatsDate { get; set; }
    public long AmountAfterVat { get; set; }
    public bool IsDebtOrder { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public string? ThumbnailUrl { get; set; }
    public OrderExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion
}
