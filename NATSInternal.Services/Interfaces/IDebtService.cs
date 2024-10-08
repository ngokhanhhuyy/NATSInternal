namespace NATSInternal.Services.Interfaces;

internal interface IDebtService<T, TUser, TUpdateHistory, TUpsertRequestDto>
    where T : class, IFinancialEngageableEntity<T, TUser, TUpdateHistory>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    Task<int> CreateAsync(TUpsertRequestDto requestDto);

    Task UpdateAsync(int id, TUpsertRequestDto requestDto);

    Task DeleteAsync(int id);
}