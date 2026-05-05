using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Orders;

public class OrderDetailResponseDto
{
    #region Constructors
    internal OrderDetailResponseDto(Order order, OrderExistingAuthorizationResponseDto authorization)
    {
        Id = order.Id;
        StatsDate = order.StatsDate;
        ProductItems = order.ProductItems.Select(pi => new OrderDetailProductItemResponseDto(pi)).ToList();
        ServiceItems = order.ServiceItems.Select(si => new OrderDetailServiceItemResponseDto(si)).ToList();
        Note = order.Note;
        CreatedDateTime = order.CreatedDateTime;
        CreatedUser = new(order.CreatedUser);
        LastUpdatedDateTime = order.LastUpdatedDateTime;
        DeletedDateTime = order.DeletedDateTime;
        Authorization = authorization;

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
    public List<OrderDetailProductItemResponseDto> ProductItems { get; }
    public List<OrderDetailServiceItemResponseDto> ServiceItems { get; }
    public string? Note { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? DeletedDateTime { get; }
    public UserBasicResponseDto? DeletedUser { get; }
    public OrderExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}