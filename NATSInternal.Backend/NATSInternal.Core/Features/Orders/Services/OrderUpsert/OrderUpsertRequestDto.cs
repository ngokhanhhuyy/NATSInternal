using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Orders;

public class OrderUpsertRequestDto : IHasStatsUpsertRequestDto
{
    #region Methods
    public OrderType Type { get; set; }
    public DateOnly? StatsDate { get; set; }
    public string? Note { get; set; }
    public int CustomerId { get; set; }
    public List<OrderUpsertProductItemRequestDto> ProductItems { get; set; } = new();
    public List<OrderUpsertServiceItemRequestDto> ServiceItems { get; set; } = new();
    public List<PhotoUpsertRequestDto> Photos { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues()
    {
        Note = Note?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}