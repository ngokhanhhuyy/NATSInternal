using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Orders;

public class OrderUpsertProductItemRequestDto : IHasProductItemUpsertRequestDto
{
    #region Properties
    public int? Id { get; set; }
    public long AmountBeforeVatPerUnit { get; set; }
    public long VatAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        if (Id.HasValue && Id.Value == 0)
        {
            Id = null;
        }
    }
    #endregion
}
