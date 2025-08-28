using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Seedwork;
using NATSInternal.Domain.Shared;

namespace NATSInternal.Infrastructure.Repositories;

internal class ListRepository
{
    #region Methods
    public async Task<Page<TEntity>> GetPagedListAsync<TEntity>(
        IQueryable<TEntity> query,
        int? page,
        int? resultsPerPage,
        CancellationToken cancellationToken = default) where TEntity : AbstractEntity
    {
        int pageOrDefault = page ?? 1;
        int resultsPerPageOrDefault = resultsPerPage ?? 15;

        int resultCount = await query.CountAsync(cancellationToken);
        if (resultCount == 0)
        {
            return new();
        }

        int pageCount = (int)Math.Ceiling((double)resultCount / resultsPerPageOrDefault);
        List<TEntity> entities = await query
            .Skip(resultsPerPageOrDefault * (pageOrDefault - 1))
            .Take(resultsPerPageOrDefault)
            .ToListAsync(cancellationToken);

        return new(entities, pageCount);
    }
    #endregion
}