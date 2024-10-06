namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductEngageableItemEntity<T, TProduct>
    : IIdentifiableEntity<T>
    where T : class, new()
    where TProduct : class, IProductEntity<TProduct>, new()
{
    long AmountPerUnit { get; set; }
    int Quantity { get; set; }
    int ProductId { get; set; }
    TProduct Product { get; set; }
}