namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableItemUpdateHistoryDataDto
    : IProductEngageableItemUpdateHistoryDataDto
{
    long VatAmountPerUnit { get; set; }
}