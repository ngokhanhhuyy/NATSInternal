namespace NATSInternal.Domain.Features.Stocks;

internal interface IStockRepository
{
    #region Methods
    Task<Stock?> GetSingleStockByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ICollection<Stock>> GetMultipleStocksByProductIdsAsync(
        IEnumerable<Guid> productIds,
        CancellationToken cancellationToken);
    #endregion
}
