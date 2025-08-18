namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasProductItemEntity<T> : IEntity<T> where T : class
{
    #region Properties
    long AmountPerUnit { get; set; }
    int Quantity { get; set; }
    Guid ProductId { get; set; }
    Product Product { get; set; }
    #endregion
}