namespace NATSInternal.Domain.Features.Supplies;

internal interface ISupplyRepository
{
    #region Methods
    Task<Supply?> GetSupplyByIdAsync(Guid id, CancellationToken cancellationToken);

    void AddSupply(Supply supply);

    void UpdateSupply(Supply supply);

    void DeleteSupply(Supply supply);
    #endregion
}