namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle the operations which are related to debt incurrence.
/// </summary>
public interface IDebtIncurrenceService
{
    /// <summary>
    /// Retrieves a list of debt incurrences with the basic information, based on the specified
    /// filtering, sorting and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="DebtListRequestDto"/> class, containing the
    /// conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// instance of the <see cref="DebtListResponseDto"/>, containing the results
    /// of the operation and the additional information for pagination.
    /// </returns>
    Task<DebtListResponseDto> GetListAsync(DebtListRequestDto requestDto);

    /// <summary>
    /// Retrieves the details of a specific debt incurrence by its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> value representing the id of the debt incurrence to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// instance of the <see cref="DebtDetailResponseDto"/>, containing the details
    /// of the debt incurrence.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the debt incurrence, specified by the value of the <paramref name="id"/>,
    /// doesn't exist or has already been deleted.
    /// </exception>
    Task<DebtDetailResponseDto> GetDetailAsync(int id);
    
    /// <summary>
    /// Creates a new debt incurrence based on the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="DebtUpsertRequestDto"/> class, containing the
    /// data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// <see cref="int"/> value representing the id of the new debt incurrence.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doens't have enough permissions to specify a value for
    /// the <c>StatsDateTime"</c> property in the <paramref name="requestDto"/>.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws under the following circumstances:
    /// - When information of the requesting user has been deleted before the operation.
    /// - When the customer which has the id specified by the value of the <c>CustomerId</c>
    /// property in the <paramref name="requestDto"/> doesn't exist or has already been
    /// deleted.
    /// </exception>
    Task<int> CreateAsync(DebtUpsertRequestDto requestDto);
    
    /// <summary>
    /// Updates an existing debt incurrence, based on its id and the specified data.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the debt incurrence to update.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="DebtUpsertRequestDto"/> class, containing the
    /// data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ValidationException">
    /// Throws when the specified value for the <c>StatsDateTime</c> property is invalid.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Throws when the debt incurrence, specified by the value of the <paramref name="id"/>,
    /// doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws under the following circumstances:<br/>
    /// - When the requesting user doesn't have enough permissions to update the specified debt
    /// incurrencesome of the new entity's properties.<br/>
    /// - When the requesting user doesn't have enough permissions to specify a value for the
    /// <c>StatsDateTime</c> property in the <param name="requestDto"/> argument.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws under the following circumstances:<br/>
    /// - When the information of the requesting user has been deleted before the operation.
    /// <br/>
    /// - When the debt incurrence information has been modified before the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - The <c>StatsDateTime</c> property in the <param name="requestDto"/> argument is
    /// specified a value when the debt incurrence has already been locked.<br/>
    /// - The remaining debt amount of the specified customer becomes negative after the
    /// operation.
    /// </exception>
    Task UpdateAsync(int id, DebtUpsertRequestDto requestDto);
    
    /// <summary>
    /// Deletes an existing debt incurrence based on its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> value representing the id of the debt incurrence to delete.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">
    /// Throws when the debt incurrence with the id specified by the <paramref name="id"/>
    /// doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to delete the specified
    /// debt incurrence.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws under the following circumstances:<br/>
    /// - When the information of the requesting user has been deleted before the operation.
    /// <br/>
    /// - When the debt incurrence has been modified before the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the remaining debt amount of the specified customer becomes negative after
    /// the operation.
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
    /// Check if the requesting user has permission to create a new debt incurrence.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();

    /// <summary>
    /// Check if the requesting user has permission to create a new
    /// <see cref="Debt"/> and retrieve the authorization information for creating
    /// operation.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="DebtCreatingAuthorizationResponseDto"/> DTO
    /// containing the authorization information for the operation when the requesting user has
    /// permission to perform the operation. Otherwise, <c>null</c>.
    /// </returns>
    DebtCreatingAuthorizationResponseDto GetCreatingAuthorization();
}