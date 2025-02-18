namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle customer-related operations.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Retrieves a list of customers with the basic information, based on the filtering,
    /// sorting and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="CustomerListRequestDto"/> class, containing the
    /// conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// an instance of the <see cref="CustomerListResponseDto"/> class, containing the results
    /// and the additional information for the pagination.
    /// </returns>
    Task<CustomerListResponseDto> GetListAsync(CustomerListRequestDto requestDto);

    /// <summary>
    /// Retrieves the basic information of a specific customer based on the id.
    /// </summary>
    /// <param name="id">
    /// A <see cref="int"/> representing the id of the customer to retrieve.
    /// </param>`
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// an instance of the <see cref="CustomerBasicResponseDto"/> class, containing the basic
    /// information of the customer.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the customer with the specified id doesn't exist or has already been
    /// deleted.
    /// </exception>
    Task<CustomerBasicResponseDto> GetBasicAsync(int id);

    /// <summary>
    /// Retrieves the details of a specific customer based on the id.
    /// </summary>
    /// <param name="id">
    /// A <see cref="int"/> representing the id of the customer to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// an instance of the <see cref="CustomerBasicResponseDto"/> class, containing the details
    /// of the customer.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the customer with the specified id doesn't exist or has already been
    /// deleted.
    /// </exception>
    Task<CustomerDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new customer based on the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="CustomerUpsertRequestDto"/> class, containing the data
    /// for the new customer.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// an <see cref="int"/> representing the id of the new customer.
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws when the customer who is this customer's introducer, specified by the value of
    /// the property <c>IntroducerId</c> in the <c>requestDto</c>, doesn't exist or has already
    /// been deleted.
    /// </exception>
    Task<int> CreateAsync(CustomerUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing customer based on the specified data.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the customer to update.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="CustomerUpsertRequestDto"/> class, containing the data
    /// for the customer to be updated.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ResourceNotFoundException">
    /// The customer with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the customer who is this customer's introducer, specified by the value of
    /// the property <c>IntroducerId</c> in the <c>requestDto</c>, doesn't exist or has already
    /// been deleted.
    /// </exception>
    Task UpdateAsync(int id, CustomerUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes an existing customer.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the customer to be deleted.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// The customer with the specified id doesn't exist or has already been deleted.
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
    /// Check if the requesting user has permission to create a new customer.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();

    /// <summary>
    /// Get statistics about the new customers this month and the ratio in percentage compared
    /// to last month.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO
    /// containing the statistics and ratio.
    /// </returns>
    Task<NewCustomerCountResponseDto> GetNewStatsAsync();
}