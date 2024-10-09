namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle update history logging related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity class with which the logging update history is associated.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity which is used to store the logging details.
/// </typeparam>
/// <typeparam name="TUpdateHistoryDataDto">
/// The type of the DTO which contains the data before and after modificiations.
/// </typeparam>
internal interface IUpdateHistoryService<T, TUpdateHistory, TUpdateHistoryDataDto>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    /// <summary>
    /// Logs the old and new data to update history for the specified entity.
    /// </summary>
    /// <param name="entity">
    /// An instance of the <see cref="T"/> entity class, representing the entity to be logged.
    /// </param>
    /// <param name="oldData">
    /// An instance of the <see cref="TUpdateHistoryDataDto"/> class, containing the data of
    /// the entity before the modification.
    /// </param>
    /// <param name="newData">
    /// An instance of the <see cref="TUpdateHistoryDataDto"/> class, containing the data of
    /// the entity after the modification.
    /// </param>
    /// <param name="reason">
    /// The reason of the modification.
    /// </param>
    void LogUpdateHistory(
            T entity,
            TUpdateHistoryDataDto oldData,
            TUpdateHistoryDataDto newData,
            string reason);
}