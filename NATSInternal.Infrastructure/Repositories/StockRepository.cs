using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Stocks;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Repositories;

internal class StockRepository : IStockRepository
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public StockRepository(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<Stock?> GetSingleStockByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Stocks.SingleOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Stock?> GetSingleStockByProductIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.Stocks.SingleOrDefaultAsync(s => s.ProductId == productId, cancellationToken);
    }

    public async Task<ICollection<Stock>> GetMultipleStocksByProductIdsAsync(
        IEnumerable<Guid> productIds,
        CancellationToken cancellationToken)
    {
        return await _context.Stocks.Where(s => productIds.Contains(s.Id)).ToListAsync(cancellationToken);
    }
    #endregion
}
