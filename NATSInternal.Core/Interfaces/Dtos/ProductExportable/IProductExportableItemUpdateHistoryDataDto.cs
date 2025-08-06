namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IProductExportableItemUpdateHistoryDataDto
    : IHasProductItemUpdateHistoryDataDto
{
    long VatAmountPerUnit { get; set; }
}