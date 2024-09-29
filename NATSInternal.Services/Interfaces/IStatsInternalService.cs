namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle the <b>internal</b> operations which are related to statistics.
/// </summary>
internal interface IStatsInternalService : IStatsService
{
    /// <summary>
    /// Validates if the specified <c>statsDateTime</c> argument is valid for an entity so that
    /// its locking status won't change after the assignment.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type which inherits from <see cref="LockableEntity"/> class.
    /// </typeparam>
    /// <param name="entity">
    /// The entity to which the <c>statsDateTime</c> argument is assigned.
    /// </param>
    /// <param name="statsDateTime">
    /// A <see cref="DateTime"/> value specified in the request representing the date and time
    /// for the field in the entity which is used to calculate the statistics.
    /// </param>
    /// <exception cref="ValidationException">
    /// Throws when the value specified by the <c>statsDateTime</c> argument is invalid.
    /// </exception>
    void ValidateStatsDateTime<TEntity>(TEntity entity, DateTime statsDateTime)
            where TEntity : LockableEntity;
}