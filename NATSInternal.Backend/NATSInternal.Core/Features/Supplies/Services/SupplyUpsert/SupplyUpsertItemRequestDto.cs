using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyUpsertItemRequestDto : IHasProductItemUpsertRequestDto
{
    #region Properties
    public int? Id { get; set; }
    public long AmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}