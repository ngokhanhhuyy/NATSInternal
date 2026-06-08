using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Payments;
using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Orders;

public class OrderDetailResponseDto
{
    #region Constructors
    internal OrderDetailResponseDto(Order order, OrderExistingAuthorizationResponseDto authorization)
    {
        Id = order.Id;
        StatsDate = order.StatsDate;
        Type = order.Type;
        ProductItems = order.ProductItems.Select(pi => new OrderProductItemDetailResponseDto(pi)).ToList();
        ServiceItems = order.ServiceItems.Select(si => new OrderServiceItemDetailResponseDto(si)).ToList();
        Note = order.Note;
        Customer = new(order.Customer);
        CreatedDateTime = order.CreatedDateTime;
        CreatedUser = new(order.CreatedUser);
        LastUpdatedDateTime = order.LastUpdatedDateTime;
        DeletedDateTime = order.DeletedDateTime;
        Photos = order.Photos.Select(p => new PhotoBasicResponseDto(p)).ToList();
        Authorization = authorization;
        AmountAfterVat = order.AmountAfterVat;
        PaidAmount = order.EffectivePayment?.Amount ?? 0;
        DebtAmount = AmountAfterVat - PaidAmount;

        if (order.EffectivePayment is not null)
        {
            Payment = new(order.EffectivePayment);
        }

        if (order.LastUpdatedUser is not null)
        {
            LastUpdatedUser = new(order.LastUpdatedUser);
        }

        if (order.DeletedUser is not null)
        {
            DeletedUser = new(order.DeletedUser);
        }
    }
    #endregion

    #region Properties
    public int Id { get; }
    public DateOnly StatsDate { get; }
    public OrderType Type { get; }
    public List<OrderProductItemDetailResponseDto> ProductItems { get; }
    public List<OrderServiceItemDetailResponseDto> ServiceItems { get; }
    public string? Note { get; }
    public CustomerBasicResponseDto Customer { get; }
    public PaymentBasicResponseDto? Payment { get; set; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? DeletedDateTime { get; }
    public UserBasicResponseDto? DeletedUser { get; }
    public List<PhotoBasicResponseDto> Photos { get; }
    public OrderExistingAuthorizationResponseDto Authorization { get; }
    public long AmountAfterVat { get; }
    public long PaidAmount { get; }
    public long DebtAmount { get; }
    #endregion
}
