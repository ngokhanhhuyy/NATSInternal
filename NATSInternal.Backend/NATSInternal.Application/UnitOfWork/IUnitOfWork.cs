namespace NATSInternal.Application.UnitOfWork;

public interface IUnitOfWork
{
    #region Methods
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    #endregion
}
