namespace NATSInternal.Services.Interfaces.Entities;

internal interface IRevenueEntity<T, TUser, TUpdateHistory>
    : IFinancialEngageableEntity<T, TUser, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUser, TUpdateHistory>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    long AmountBeforeVat { get; }
    long VatAmount { get; }
}