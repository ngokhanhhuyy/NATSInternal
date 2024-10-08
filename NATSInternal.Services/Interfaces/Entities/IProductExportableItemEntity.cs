namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductExportableItemEntity<T> : IProductEngageableItemEntity<T>
    where T : class, IProductEngageableItemEntity<T>, new()
{
    long VatAmountPerUnit { get; set; }
}