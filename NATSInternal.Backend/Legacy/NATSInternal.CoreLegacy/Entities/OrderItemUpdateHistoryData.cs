namespace NATSInternal.Core.Entities;

internal class OrderItemUpdateHistoryData
{
    #region Properties
    public required Guid Id { get; set; }
    public required long AmountBeforeVatPerUnit { get; set; }
    public required long VatAmountPerUnit { get; set; }
    public required int Quantity { get; set; }
    public required Guid ProductId { get; set; }
    public required string ProductName { get; set; }
    #endregion
}
