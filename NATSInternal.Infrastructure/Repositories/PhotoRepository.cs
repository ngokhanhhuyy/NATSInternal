using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Repositories;

internal class PhotoRepository : IPhotoRepository
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public PhotoRepository(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<Photo?> GetSinglePhotoByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Photos.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<ICollection<Photo>> GetMultiplePhotosByProductIdsAsync(
        IEnumerable<Guid> productIds,
        CancellationToken cancellationToken)
    {
        return await _context.Photos
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<Photo>> GetMultiplePhotosByBrandIdsAsync(
        IEnumerable<Guid> brandIds,
        CancellationToken cancellationToken)
    {
        return await _context.Photos
            .Where(p => brandIds.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }
    #endregion
}
