namespace NATSInternal.Services.Dtos;

/// <summary>
/// Service to handle consultant-related operations.
/// </summary>
public interface IConsultantService
{
    /// <summary>
    /// Retrieves a list of consultant, based on the specified filtering, sorting and
    /// paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="ConsultantListRequestDto"/> class, containing the
    /// conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// instance of the <see cref="ConsultantListResponseDto"/>, containing the results.
    /// </returns>
    Task<ConsultantListResponseDto> GetListAsync(ConsultantListRequestDto requestDto);

    /// <summary>
    /// Retrieves a specific consultant, specified by its id.
    /// </summary>
    /// <param name="id">An <see cref="int"/> representing the id of the consultant.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation, which result is an
    /// instance of the <see cref="ConsultantDetailResponseDto"/> class, containing the
    /// details of the consultant.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the consultant with the specified id doesn't exist or has already been
    /// deleted.
    /// </exception>
    Task<ConsultantDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new consultant with the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="ConsultantUpsertRequestDto"/> class, containing the data
    /// for the operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> where <c>TResult</c> is an <see cref="int"/>,
    /// representing the id of the new
    /// consultant.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Thrown when the user doens't have enough permissions to update the consultant or
    /// to specify a value for the property <c>StatsDateTime</c> in the <c>requestDto</c>
    /// argument.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrecy-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the requesting user has been deleted before the operation.<br/>
    /// - When the customer with id specified by the value of the property
    /// <c>CustomerId</c> in the <c/>requestDto<c/> argument doesn't exist or has already been
    /// deleted.<br/>
    /// </exception>
    Task<int> CreateAsync(ConsultantUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing consultant with the specified data.
    /// </summary>
    /// <param name="id">An <see cref="int"/> representing the id of the consultant.</param>
    /// <param name="requestDto">
    /// An instance of the <see cref="ConsultantUpsertRequestDto"/> class, containing the
    /// data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the consultant with the specified id doens't exist or has already been
    /// deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the user doens't have enough permissions to update the consultant or
    /// to specify a value for the property <c>StatsDateTime</c> in the <c>requestDto</c>
    /// argument.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws the operation faces concurrency conflict.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the value for <c>StatsDateTime</c> property in the <c>requestDto</c>
    /// argument is specified when the consultant has already been locked.<br/>
    /// - When the requesting user has been deleted before the operation.<br/>
    /// - When the customer with id specified by the value of the property
    /// <c>CustomerId</c> in the <c/>requestDto<c/> argument doesn't exist or has already been
    /// deleted.<br/>
    /// </exception>
    Task UpdateAsync(int id, ConsultantUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes an existing consultant by its id.
    /// </summary>
    /// <param name="id">
    /// A <see cref="int"/> representing the id of the consultant to delete.
    /// </param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the consultant with the specified id doesn't exist or has already been
    /// deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related confict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the consultant's deletion is restricted due to the existence of some
    /// related data.
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
    /// Retrieve a list of the <see cref="MonthYearResponseDto"/> instances, representing the
    /// options and the default option that users can select as filtering condition in list
    /// retrieving operation.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// a <see cref="List{T}"/> of <see cref="MonthYearOptionsResponseDto"/> DTO, representing
    /// the options.
    /// </returns>
    Task<MonthYearOptionsResponseDto> GetListMonthYearOptionsAsync();

    /// <summary>
    /// Check if the requesting user has permission to create a new consultant.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();

    /// <summary>
    /// Check if the requesting user has permission to create a new consultant and retrieve the
    /// authorization information for creating operation.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="ConsultantNewAuthorizationResponseDto"/> DTO containing
    /// the authorization information for the operation when the requesting user has permission
    /// to perform the operation. Otherwise, <c>null</c>.
    /// </returns>
    ConsultantNewAuthorizationResponseDto GetCreatingAuthorization();
}