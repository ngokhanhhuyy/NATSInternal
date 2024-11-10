namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableItemUpdateHistoryDataDto
    : IHasProductItemUpdateHistoryDataDto
{
    long VatAmountPerUnit { get; set; }
}