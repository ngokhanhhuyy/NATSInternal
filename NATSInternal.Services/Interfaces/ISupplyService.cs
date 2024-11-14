namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle supplies.
/// </summary>
public interface ISupplyService
{
    /// <summary>
    /// Gets a list of supplies with filtering conditions for the results.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="SupplyListRequestDto"/> containing values for the
    /// filtering conditions and pagination.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> which resolves to an instance of the
    /// <see cref="SupplyListResponseDto"/>, containing the results and some information for
    /// pagination.
    /// </returns>
    Task<SupplyListResponseDto> GetListAsync(SupplyListRequestDto requestDto);

    /// <summary>
    /// Gets the detailed information of the supply with specified id.
    /// </summary>
    /// <param name="id">The id of the supply.</param>
    /// <returns>
    /// An instance of the <see cref="SupplyDetailResponseDto"/> containing the detailed
    /// information of the supply.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the supply which has the specified <c>id</c> doesn't exist or has already
    /// been deleted.
    /// </exception>
    Task<SupplyDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a supply with the data provided in the request.
    /// </summary>
    /// <param name="requestDto">An object containing the data for the </param>
    /// <returns>
    /// A <see cref="Task"/> which resolves to an <see cref="int"/> representing the id of the
    /// created supply.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to perform the
    /// operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the <see cref="Product"/> which has the specified <c>ProductId</c> in the
    /// provided <c>requestDto</c> doesn't exist.
    /// </exception>
    Task<int> CreateAsync(SupplyUpsertRequestDto requestDto);

    /// <summary>
    /// Updates the supply which has the given id with the data provided from the request.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the supply to be updated.
    /// </param>
    /// <param name="requestDto">An object containing the data the be updated.</param>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to perform the
    /// operation.
    /// </exception>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the supply with the specified <c>id</c> doesn't exist or has already been
    /// deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the <see cref="Product"/> which has the specified <c>ProductId</c> in the
    /// provided <c>requestDto</c> doesn't exist.
    /// </exception>
    Task UpdateAsync(int id, SupplyUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes the supply with given id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the supply to be deleted.
    /// </param>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the supply with the specified <c>id</c> doesn't exist or has already been
    /// deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the supply's deletion is restricted due to the existence of some related
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
    /// Check if the requesting user has permission to create a new supply.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();

    /// <summary>
    /// Check if the requesting user has permission to create a new supply and retrieve the
    /// authorization information for creating operation.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="SupplyCreatingAuthorizationResponseDto"/> DTO containing the
    /// authorization information for the operation when the requesting user has permission
    /// to perform the operation. Otherwise, <c>null</c>.
    /// </returns>
    SupplyCreatingAuthorizationResponseDto GetCreatingAuthorization();
}
