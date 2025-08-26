namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasCustomerEntity<T, TData> : IHasStatsEntity<T, TData>
    where T : class, IUpsertableEntity<T>
    where TData : class
{
    #region Properties
    Guid CustomerId { get; set; }
    Customer Customer { get; set; }
    #endregion
}