namespace NATSInternal.Core.Features.Customers;

/// <summary>
/// Defines operations for listing, retrieving, creating, updating, and deleting customers.
/// </summary>
public interface ICustomerService
{
    #region Properties
    /// <summary>
    /// Retrieves a paginated list of active customers that match the supplied filter and sort criteria.
    /// </summary>
    /// <param name="requestDto">
    /// The request containing search text, sort options, page number, and page size.
    /// </param>
    /// <returns>
    /// A task that returns the paginated customer list response with items, page count, and total item count.
    /// </returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when <paramref name="requestDto"/> fails validation.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Thrown when the requested sort field is not implemented by the service.
    /// </exception>
    /// <example>
    /// <code>
    /// CustomerListResponseDto customers = await customerService.GetListAsync(new CustomerListRequestDto
    /// {
    ///     SearchContent = "nguyen",
    ///     SortByFieldName = nameof(CustomerListRequestDto.FieldToSort.LastName),
    ///     SortByAscending = true,
    ///     Page = 1,
    ///     ResultsPerPage = 20
    /// });
    /// </code>
    /// </example>
    Task<CustomerListResponseDto> GetListAsync(CustomerListRequestDto requestDto);

    /// <summary>
    /// Retrieves the detailed information of a customer by identifier.
    /// </summary>
    /// <param name="id">The identifier of the customer to retrieve.</param>
    /// <returns>
    /// A task that returns the customer detail response for the matching customer.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no customer exists for <paramref name="id"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// CustomerDetailResponseDto customer = await customerService.GetDetailAsync(15);
    /// </code>
    /// </example>
    Task<CustomerDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new customer from the supplied data and returns the created customer identifier.
    /// </summary>
    /// <param name="requestDto">
    /// The request containing the customer information to persist.
    /// </param>
    /// <returns>
    /// A task that returns the identifier assigned to the newly created customer.
    /// </returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when <paramref name="requestDto"/> fails validation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when the specified introducer does not exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when the current caller context becomes invalid during persistence.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when another customer already uses the supplied nickname.
    /// </exception>
    /// <example>
    /// <code>
    /// int customerId = await customerService.CreateAsync(new CustomerUpsertRequestDto
    /// {
    ///     FirstName = "An",
    ///     LastName = "Nguyen",
    ///     NickName = "annguyen",
    ///     PhoneNumber = "0900000000"
    /// });
    /// </code>
    /// </example>
    Task<int> CreateAsync(CustomerUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing active customer with the supplied data.
    /// </summary>
    /// <param name="id">The identifier of the customer to update.</param>
    /// <param name="requestDto">
    /// The request containing the updated customer information.
    /// </param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when <paramref name="requestDto"/> fails validation.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Thrown when no active customer exists for <paramref name="id"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to edit the specified customer.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when the specified introducer does not exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict occurs while saving the customer changes.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when another customer already uses the supplied nickname.
    /// </exception>
    /// <example>
    /// <code>
    /// await customerService.UpdateAsync(15, new CustomerUpsertRequestDto
    /// {
    ///     FirstName = "An",
    ///     LastName = "Tran",
    ///     NickName = "an.tran",
    ///     Email = "an.tran@example.com"
    /// });
    /// </code>
    /// </example>
    Task UpdateAsync(int id, CustomerUpsertRequestDto requestDto);

    /// <summary>
    /// Soft deletes an existing active customer.
    /// </summary>
    /// <param name="id">The identifier of the customer to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no active customer exists for <paramref name="id"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to delete the specified customer.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict occurs while saving the delete operation.
    /// </exception>
    /// <example>
    /// <code>
    /// await customerService.DeleteAsync(15);
    /// </code>
    /// </example>
    Task DeleteAsync(int id);
    #endregion
}
