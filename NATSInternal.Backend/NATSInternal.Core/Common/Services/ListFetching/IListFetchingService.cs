namespace NATSInternal.Core.Common.Services;

internal interface IListFetchingService
{
    #region Methods
    Task<Page<TResult>> GetPagedListAsync<TResult>(IQueryable<TResult> query, int? page, int? resultsPerPage);
    #endregion
}
