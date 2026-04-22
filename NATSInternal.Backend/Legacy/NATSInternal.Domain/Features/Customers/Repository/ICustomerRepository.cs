namespace NATSInternal.Domain.Features.Customers;

internal interface ICustomerRepository
{
    #region Methods
    Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerByIdIncludingIntroducerAsync(Guid id, CancellationToken cancellationToken = default);
    void AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(Customer customer);
    #endregion
}
