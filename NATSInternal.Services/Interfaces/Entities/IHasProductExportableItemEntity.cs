namespace NATSInternal.Services.Interfaces.Entities;

internal interface IHasProductExportableItemEntity<T> : IHasProductItemEntity<T>
    where T : class, IHasProductItemEntity<T>, new()
{
    long VatAmountPerUnit { get; set; }
}