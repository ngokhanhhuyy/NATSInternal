namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasProductItemUpdateHistoryDataDto
{
    #region Properties
    Guid Id { get; }
    long AmountBeforeVatPerUnit { get; }
    int Quantity { get; }
    string ProductName { get; }
    #endregion
}