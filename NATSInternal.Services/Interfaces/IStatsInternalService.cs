namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle the <b>internal</b> operations which are related to statistics.
/// </summary>
internal interface IStatsInternalService : IStatsService
{
    /// <summary>
    /// Increases the retail revenue statistics for a specific date or today if not specified.
    /// </summary>
    /// <param name="value">
    /// The amount by which to increment the retail revenue.
    /// </param>
    /// <param name="date">
    /// Optional. The date for which to update the statistics. If not provided, today's date is
    /// used.
    /// </param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method updates both the daily and monthly retail revenue statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately after the increment operation.
    /// </remarks>
    Task IncrementRetailGrossRevenueAsync(long value, DateOnly? date = null);

    /// <summary>
    /// Increases the retail revenue statistics for a specific date or today if not specified.
    /// </summary>
    /// <param name="value">
    /// The amount by which to increment the retail revenue.
    /// </param>
    /// <param name="date">
    /// Optional. The date for which to update the statistics. If not provided, today's date is
    /// used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method updates both the daily and monthly retail revenue statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately after the increment operation.
    /// </remarks>
    Task IncrementTreatmentGrossRevenueAsync(long value, DateOnly? date = null);

    /// <summary>
    /// Increases the consultant revenue statistics for a specific date or today if not
    /// specified.
    /// </summary>
    /// <param name="value">
    /// The amount by which to increment the consultant revenue.
    /// </param>
    /// <param name="date">
    /// Optional. The date for which to update the statistics. If not provided, today's date is
    /// used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method updates both the daily and monthly consultant revenue statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately after the increment operation.
    /// </remarks>
    Task IncrementConsultantGrossRevenueAsync(long value, DateOnly? date = null);
    
    /// <summary>
    /// Increases the debt incurrence amount statistics for a specific date or today if not
    /// specified.
    /// </summary>
    /// <param name="value">
    /// The amount by which to increment the debt amount.
    /// </param>
    /// <param name="date">
    /// (Optional) The date for which the statistics is updated. If not specified, today's date
    /// is used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method updates both the daily and monthly debt amount statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately
    /// after the increment operation.
    /// </remarks>
    Task IncrementDebtIncurredAmountAsync(long value, DateOnly? date = null);

    /// <summary>
    /// Increases the debt paid amount statistics for a specific date or today if not
    /// specified.
    /// </summary>
    /// <param name="amount">
    /// The amount by which to increment the debt paid amount.
    /// </param>
    /// <param name="date">
    /// (Optional) The date for which to update the statistics. If not provided, today's date
    /// is used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method updates both the daily and monthly debt paid amount statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately after the increment operation.
    /// </remarks>
    Task IncrementDebtPaidAmountAsync(long amount, DateOnly? date = null);

    /// <summary>
    /// Increases the vat collected amount statistics for a specific date or today if not
    /// specified.
    /// </summary>
    /// <param name="amount">
    /// The amount by which to increment the vat collected amount.
    /// </param>
    /// <param name="date">
    /// Optional. The date for which to update the statistics. If not provided, today's date is
    /// used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method updates both the daily and monthly vat collected amount statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately after the increment operation.
    /// </remarks>
    Task IncrementVatCollectedAmountAsync(long amount, DateOnly? date = null);

    /// <summary>
    /// Increases the shipment cost statistics for a specific date or today if not specified.
    /// </summary>
    /// <param name="value">
    /// The amount by which to increment the shipment cost.
    /// </param>
    /// <param name="date">
    /// Optional. The date for which to update the statistics. If not provided, today's date is
    /// used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method updates both the daily and monthly shipment cost statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately after the increment operation.
    /// </remarks>
    Task IncrementShipmentCostAsync(long value, DateOnly? date = null);
    
    /// <summary>
    /// Increases the expense with given category statistics for a specific date or today if
    /// not specified.
    /// </summary>
    /// <param name="value">
    /// The amount by which to increment the expense.
    /// </param>
    /// <param name="category">
    /// The category of expense to increment.
    /// </param>
    /// <param name="date">
    /// Optional. The date for which to update the statistics. If not provided, today's date is
    /// used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method updates both the daily and monthly expense statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately after the increment operation.
    /// </remarks>
    Task IncrementExpenseAsync(long value, ExpenseCategory category, DateOnly? date = null);

    /// <summary>
    /// Increases the supply cost statistics for a specific date or today if not specified.
    /// </summary>
    /// <param name="value">
    /// The amount by which to increment the supply cost.
    /// </param>
    /// <param name="date">
    /// Optional. The date for which to update the statistics. If not provided, today's date is
    /// used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method updates both the daily and monthly supply cost statistics. If
    /// <paramref name="date"/> is not specified, the statistics for today are updated. The
    /// changes are persisted to the database immediately after the increment operation.
    /// </remarks>
    Task IncrementSupplyCostAsync(long value, DateOnly? date = null);

    /// <summary>
    /// Increses the new customer cost statistics for a specific date or today if not
    /// specified.
    /// </summary>
    /// <param name="value">The number of the new customers to increment.</param>
    /// <param name="date">
    /// Optional. The date for which to update the statistics. If not provided, today's date is
    /// used.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task IncrementNewCustomerAsync(int value, DateOnly? date = null);
    
    /// <summary>
    /// Close daily stats by given date. This will write the datetime value when this method is
    /// called to the daily stats <c>TemporarilyClosedDateTime</c> property.
    /// </summary>
    /// <param name="date">The date of daily stats which should be closed.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task TemporarilyCloseAsync(DateOnly date);

    /// <summary>
    /// Get the minimum datetime that if a resource is assigned to, it will be considered
    /// <b>opened</b>.
    /// If it earlier than the current month's 4th 02:00, the minimum datetime will be 00:00 of
    /// the first day of 2 months ago. Otherwise, it will be the 00:00 of the first day of the
    /// last month.
    /// </summary>
    /// <returns>
    /// The minimum datetime that if a resource is assigned to, it will be considered
    /// <b>opened</b>.
    /// </returns>
    DateTime GetResourceMinimumOpenedDateTime();

    /// <summary>
    /// Validates if the specified <paramref name="statsDateTime"/> is valid for an entity so
    /// that its locking status won't change after the assignment.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the entity class to which the stats belongs.
    /// </typeparam>
    /// <typeparam name="TUpdateHistory">
    /// The type of the update history with which the stats is associated.
    /// </typeparam>
    /// <param name="entity">
    /// An instance of the entity class to which the <paramref name="statsDateTime"/> is
    /// assigned.
    /// </param>
    /// <param name="statsDateTime">
    /// A <see cref="DateTime"/> value specified in the request representing the date and time
    /// for the field in the entity which is used to calculate the statistics.
    /// </param>
    /// <exception cref="ValidationException">
    /// Throws when the value specified by the <paramref name="statsDateTime"/> is invalid.
    /// </exception>
    void ValidateStatsDateTime<T, TUpdateHistory>(T entity, DateTime statsDateTime)
        where T : class, IHasStatsEntity<T, TUpdateHistory>, new()
        where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new();
}