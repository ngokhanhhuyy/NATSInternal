namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle month-year and month-year options related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity which <c>StatsDateTime</c> is based on.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity which is associated with the <typeparamref name="T"/> entity.
/// </typeparam>
internal interface IMonthYearService<T, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    /// <summary>
    /// Generates a list of the <see cref="MonthYearResponseDto"/> instances, representing the
    /// options that users can select as filtering condition when fetching a list of
    /// <typeparamref name="T"/> DTOs.
    /// </summary>
    /// <param name="repositorySelector">
    /// A <see cref="Func{T, TResult}"/> that selects the <typeparamref name="T"/> repository in the
    /// <see cref="DatabaseContext"/> instance.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of <see cref="MonthYearResponseDto"/> instances, representing
    /// the options.
    /// </returns>
    Task<List<MonthYearResponseDto>> GenerateMonthYearOptions(
        Func<DatabaseContext, DbSet<T>> repositorySelector);
}