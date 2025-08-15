namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasProductItemEntity<T> : IEntity<T> where T : class, new()
{
    long ProductAmountPerUnit { get; set; }
    int Quantity { get; set; }
    int ProductId { get; set; }
    Product Product { get; set; }
}