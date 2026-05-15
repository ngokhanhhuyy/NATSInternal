using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Orders;

public class OrderUpsertRequestDto : IHasStatsUpsertRequestDto
{
    #region Properties
    public OrderType Type { get; set; }
    public DateOnly? StatsDate { get; set; }
    public string? Note { get; set; }
    public long PaidAmount { get; set; }
    public int? CustomerId { get; set; }
    public CustomerUpsertRequestDto Customer { get; set; } = null!;
    public List<OrderUpsertProductItemRequestDto> ProductItems { get; set; } = new();
    public List<OrderUpsertServiceItemRequestDto> ServiceItems { get; set; } = new();
    public List<PhotoUpsertRequestDto> Photos { get; set; } = new();
    #endregion

    #region ComputedProperties
    internal long ProductItemsAmount => ProductItems.Sum(pi =>
    {
        return (pi.AmountBeforeVatPerUnit + pi.VatAmountPerUnit) * pi.Quantity;
    });
    
    internal long ServiceItemsAmount => ServiceItems.Sum(si =>
    {
        return (si.AmountBeforeVatPerUnit + si.VatAmountPerUnit) * si.Quantity;
    });

    internal long Amount => ProductItemsAmount + ServiceItemsAmount;
    #endregion

    #region Methods
    public void TransformValues()
    {
        Note = Note?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}
