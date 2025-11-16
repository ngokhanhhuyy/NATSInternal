namespace NATSInternal.Domain.Features.Customers;

internal interface ICustomerRepository
{
    #region Methods
    Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken);
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    #endregion
}
