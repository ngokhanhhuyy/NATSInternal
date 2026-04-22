namespace NATSInternal.Infrastructure.Services;

public interface IListFetchingService
{
    #region Methods
    Task<Page<TResult>> GetPagedListAsync<TResult>(
        IQueryable<TResult> query,
        int? page,
        int? resultsPerPage,
        CancellationToken cancellationToken = default);
    #endregion
}
