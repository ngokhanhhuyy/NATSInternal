using Microsoft.EntityFrameworkCore;

namespace NATSInternal.Core.Common.Services;

internal class ListFetchingService : IListFetchingService
{
    #region Methods
    public async Task<Page<TEntity>> GetPagedListAsync<TEntity>(
        IQueryable<TEntity> query,
        int? page,
        int? resultsPerPage)
    {
        int pageOrDefault = page ?? 1;
        int resultsPerPageOrDefault = resultsPerPage ?? 15;

        int resultCount = await query.CountAsync();
        if (resultCount == 0)
        {
            return new();
        }

        int pageCount = (int)Math.Ceiling((double)resultCount / resultsPerPageOrDefault);
        List<TEntity> entities = await query
            .Skip(resultsPerPageOrDefault * (pageOrDefault - 1))
            .Take(resultsPerPageOrDefault)
            .ToListAsync();

        return new(entities, pageCount, resultCount);
    }
    #endregion
}