namespace NATSInternal.Core.Features.Customers;

internal interface ICustomerInternalService : ICustomerService
{
    #region Methods
    Task<Customer> GetOrCreateAsync(int? id, CustomerUpsertRequestDto requestDto);
    Task UpdateCachedRemaningDebtAmountAsync(Customer customer, Func<long, long> getAmount);
    #endregion
}