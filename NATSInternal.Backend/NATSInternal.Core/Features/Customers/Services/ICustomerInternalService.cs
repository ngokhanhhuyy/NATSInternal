namespace NATSInternal.Core.Features.Customers;

internal interface ICustomerInternalService : ICustomerService
{
    #region Methods
    Task<Customer> GetOrCreateAsync(int? id, CustomerUpsertRequestDto requestDto);
    Task UpdateCachedRemaningDebtAmountAsync(int id, Func<long, long> getAmount);
    Task UpdateCachedDebtAmount(Customer customer, Func<long, long> getAmount);
    #endregion
}
