namespace NATSInternal.Services.Interfaces.Entities;

internal interface IExportProductItemEntity<T> : IHasProductItemEntity<T>
    where T : class, IHasProductItemEntity<T>, new()
{
    long VatAmountPerUnit { get; set; }
}