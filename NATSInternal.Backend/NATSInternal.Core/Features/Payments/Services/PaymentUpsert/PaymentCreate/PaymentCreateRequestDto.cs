namespace NATSInternal.Core.Features.Payments;

public class PaymentCreateRequestDto : AbstractPaymentUpsertRequestDto
{
    #region Properties
    public int CustomerId { get; set; }
    public int? OrderId { get; set; }
    #endregion
}
