namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductExportableItemEntity<T, TProduct>
    : IProductEngageableItemEntity<T, TProduct>
    where T : class, IProductEngageableItemEntity<T, TProduct>, new()
    where TProduct : class, IProductEntity<TProduct>, new()
{
    long VatAmountPerUnit { get; set; }
}