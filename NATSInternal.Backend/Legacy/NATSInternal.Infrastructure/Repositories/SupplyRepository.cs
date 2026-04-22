using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Supplies;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Repositories;

internal class SupplyRepository : ISupplyRepository
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public SupplyRepository(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<Supply?> GetSupplyByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Supplies.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public void AddSupply(Supply supply)
    {
        _context.Supplies.Add(supply);
    }
    
    public void UpdateSupply(Supply supply)
    {
        _context.Supplies.Add(supply);
    }

    public void DeleteSupply(Supply supply)
    {
        _context.Supplies.Add(supply);
    }
    #endregion
}