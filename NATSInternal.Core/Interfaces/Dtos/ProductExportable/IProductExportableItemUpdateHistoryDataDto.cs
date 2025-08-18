namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IProductExportableItemUpdateHistoryDataDto : IHasProductItemUpdateHistoryDataDto
{
    #region Properties
    long VatAmountPerUnit { get; set; }
    #endregion
}