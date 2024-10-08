namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductEngageableItemEntity<T>
    : IIdentifiableEntity<T>
    where T : class, new()
{
    long AmountPerUnit { get; set; }
    int Quantity { get; set; }
    int ProductId { get; set; }
    Product Product { get; set; }
}