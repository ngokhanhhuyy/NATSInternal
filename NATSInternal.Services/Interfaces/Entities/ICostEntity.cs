namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICostEntity<T, TUpdateHistory>
    : IFinancialEngageableEntity<T, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    long Amount { get; set; }
}
