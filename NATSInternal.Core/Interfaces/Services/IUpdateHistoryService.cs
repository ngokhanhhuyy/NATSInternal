namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle update history logging related operations.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity class with which the logging update history is associated.
/// </typeparam>
/// <typeparam name="TData">
/// The type of the DTO which contains the data before and after modificiations.
/// </typeparam>
internal interface IUpdateHistoryService<TEntity, TData>
    where TEntity : class, IHasStatsEntity<TEntity, TData>
    where TData : class
{
    /// <summary>
    /// Add the old and new data to update history for the specified entity.
    /// </summary>
    /// <param name="entity">
    /// An instance of the <typeparamref name="TEntity"/> entity class, representing the entity to be logged.
    /// </param>
    /// <param name="oldData">
    /// An instance of the <typeparamref name="TData"/> class, containing the data of the entity before the
    /// modification.
    /// </param>
    /// <param name="newData">
    /// An instance of the <typeparamref name="TData"/> class, containing the data of the entity after the modification.
    /// </param>
    /// <param name="reason">
    /// The reason of the modification.
    /// </param>
    void AddUpdateHistory(TEntity entity, TData oldData, TData newData, string reason);
}