namespace NATSInternal.Core.Interfaces;

/// <summary>
/// A service to handle the expense-related operations.
/// </summary>
public interface IExpenseService
{
    /// <summary>
    /// Retrieves a list of expenses with the basic information, based on the specified
    /// filtering, sorting and paginating
    /// conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="ExpenseListRequestDto"/> class, containing conditions
    /// for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing asynchronous operation, which result is an instance
    /// of the <see cref="ExpenseListRequestDto"/> class, containing the result and some
    /// additional information for the pagination.
    /// </returns>
    Task<ExpenseListResponseDto> GetListAsync(ExpenseListRequestDto requestDto);

    /// <summary>
    /// Retrieves the details of a specific expense by its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the expense to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// instance of the <see cref="ExpenseDetailResponseDto"/> class, containing the details
    /// of the expense.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the expense with the specified id doesn't exist or has already been
    /// deleted.
    /// </exception>
    Task<ExpenseDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new expense with the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="ExpenseUpsertRequestDto"/> class, containing the data
    /// for the new expense.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// <see cref="int"/> representing the id of the new expense.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the value for the <c>StatsDateTime</c> property has been provided in the
    /// <c>requestDto</c>, but the requesting user doesn't have enough permissions to do so.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when a requesting-user-related conflict or when a payee-related conflict occurs
    /// during the operation.
    /// </exception>
    Task<int> CreateAsync(ExpenseUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing expense with the specified data.
    /// </summary>
    /// <param name="id">
    /// The ID of the expense to update.
    /// </param>
    /// <param name="requestDto">
    /// The data transfer object containing the updated details of the expense.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the expense with the specified id doens't exist or has already been
    /// deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the user is not authorized to edit the expense.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the user who is requesting has been deleted or when a payee-related
    /// conflict occurs during the operation.
    /// </exception>
    Task UpdateAsync(int id, ExpenseUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes an existing expense by its id.
    /// </summary>
    /// <param name="id">The ID of the expense to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the user doesn't have permission to delete the specifed expense.
    /// </exception>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the expense with the specified id doens't exist or has already been
    /// deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the expense's deletion is restricted due to the existence of some related
    /// data.
    /// </exception>
    Task DeleteAsync(int id);

    /// <summary>
    /// Get all fields those are used as options to order the results in list retrieving
    /// operation.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="ListSortingOptionsResponseDto"/> DTO, containing the
    /// options with name and display names of the fields and the default field.
    /// </returns>
    ListSortingOptionsResponseDto GetListSortingOptions();

    /// <summary>
    /// Retrieve month year options which user can select as the filtering condition and the
    /// default option, used in the list retrieving operation.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which is an instance of
    /// the <see cref="ListMonthYearOptionsResponseDto"/> DTO, containing the options.
    /// </returns>
    Task<ListMonthYearOptionsResponseDto> GetListMonthYearOptionsAsync();

    /// <summary>
    /// Check if the requesting user has permission to create a new expense.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();

    /// <summary>
    /// Check if the requesting user has permission to create a new <see cref="Expense"/>
    /// and retrieve the authorization information for creating operation.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="ExpenseCreatingAuthorizationResponseDto"/> DTO containing
    /// the authorization information for the operation when the requesting user has permission
    /// to perform the operation. Otherwise, <c>null</c>.
    /// </returns>
    ExpenseCreatingAuthorizationResponseDto GetCreatingAuthorization();
}