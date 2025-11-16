using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Customers
            .Where(c => c.Id == id && c.DeletedDateTime == null)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
    }
    #endregion
}
