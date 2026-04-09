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
    public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetCustomerByIdIncludingIntroducerAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.Introducer)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public void DeleteCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
    }
    #endregion
}
