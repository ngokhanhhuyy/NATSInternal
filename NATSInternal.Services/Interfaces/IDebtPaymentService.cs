namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle the operations which are related to debt payment.
/// </summary>
public interface IDebtPaymentService
{
    /// <summary>
    /// Retrieves a list of debt payments with the basic information, based on the specified
    /// filtering, sorting and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="DebtPaymentListRequestDto"/> class, containing the
    /// conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// instance of the <see cref="DebtPaymentListResponseDto"/>, containing the results
    /// of the operation and the additional information for pagination.
    /// </returns>
    Task<DebtPaymentListResponseDto> GetListAsync(DebtPaymentListRequestDto requestDto);

    /// <summary>
    /// Retrieves the details of a specific debt payment by its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> value representing the id of the debt payment to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// instance of the <see cref="DebtPaymentDetailResponseDto"/> class, containing the
    /// details of the debt payment.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the debt payment with the specified <paramref name="id"/>s doesn't exist or
    /// has already been deleted.
    /// </exception>
    Task<DebtPaymentDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new debt payment based on the specified data from the request.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="DebtPaymentUpsertRequestDto"/> class, containing the data
    /// for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asychronous operation, which result is an
    /// <see cref="int"/> representing the id of the new debt payment.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to specify a value
    /// for the <c>StatsDateTime</c> property in the <param name="requestDto"/> argument.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when the information of the requesting user has already been deleted before the
    /// operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - The customer specified by the <c>CustomerId</c> in the <paramref name="requestDto"/>
    /// doesn't exist or has already been deleted.
    /// - The remaining debt amount of the specified customer becomes negative after the
    /// operation.
    /// </exception>
    Task<int> CreateAsync(DebtPaymentUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing debt payment, based on its id and the provided data.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> value representing the id of the debt payment to update.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="DebtPaymentUpsertRequestDto"/> class, containing the data
    /// for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the debt payment specified by the <paramref name="id"/> doesn't exist or
    /// has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws under the following circumstances:<br/>
    /// - When the requesting user doesn't have enough permissions to update the debt payment.
    /// - When the requesting user can update the debt payment, but doesn't have enough
    /// permissions to specify a value for the <c>StatsDateTime</c> property in the
    /// <paramref name="requestDto"/>.
    /// </exception>
    /// <exception cref="ValidationException">
    /// Throws when the value of the <c>StatsDateTime</c> property in the <c>requestDto</c>
    /// argument is invalid.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the <c>StatsDateTime</c> property in the <param name="requestDto"/> argument is
    /// specified
    /// a value while the debt payment has already been locked.
    /// - When the remaining debt amount of the associated customer becomes negative after the
    /// operation.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws under the following circumstances:<br/>
    /// - When the debt payment has been modified by another process before the operation.<br/>
    /// - When the information of the requesting user has been deleted by another process
    /// before the operation.
    /// </exception>
    Task UpdateAsync(int id, DebtPaymentUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes an existing debt payment, specified by its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> value representing the id of the debt payment to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the debt payment specified by the <c>customerId</c> and the
    /// <paramref name="id"/> doesn't exist.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to delete the specified
    /// debt payment.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the debt payment is locked.
    /// - When the specified customer's remaining debt amount becomes negative after the
    /// operation.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
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
    /// Check if the requesting user has permission to create a new debt payment.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();

    /// <summary>
    /// Check if the requesting user has permission to create a new
    /// <see cref="DebtPayment"/> and retrieve the authorization information for creating
    /// operation.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="DebtPaymentCreatingAuthorizationResponseDto"/> DTO
    /// containing the authorization information for the operation when the requesting user has
    /// permission to perform the operation. Otherwise, <c>null</c>.
    /// </returns>
    DebtPaymentCreatingAuthorizationResponseDto GetCreatingAuthorization();
}