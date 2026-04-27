namespace NATSInternal.Core.Features.Products;

/// <summary>
/// Service interface for managing product operations.
/// Provides methods for retrieving, creating, updating, and deleting products.
/// </summary>
public interface IProductService
{
    #region Properties
    /// <summary>
    /// Retrieves a paginated list of products based on the specified request criteria.
    /// </summary>
    /// <param name="requestDto">The request containing filtering, sorting, and pagination parameters.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the product
    /// list response with items, total count, and page count.
    /// </returns>
    /// <exception cref="ValidationException">Thrown when the requestDto fails validation.</exception>
    /// <exception cref="ArgumentException">Thrown when an invalid sort field is specified.</exception>
    Task<ProductListResponseDto> GetListAsync(ProductListRequestDto requestDto);

    /// <summary>
    /// Retrieves detailed information for a specific product by its id.
    /// </summary>
    /// <param name="id">The unique identifier of the product to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, containing the product detail response.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the product with the specified id is not found or has been deleted.
    /// </exception>
    Task<ProductDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new product with the specified details.
    /// </summary>
    /// <param name="requestDto">The request containing the product creation data.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the id of the newly created product.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user does not have permission to create products.
    /// </exception>
    /// <exception cref="ValidationException">Thrown when the requestDto fails validation.</exception>
    /// <exception cref="OperationException">
    /// Thrown when one or more category IDs do not exist in the system.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a foreign key constraint violation occurs during save.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when a product with the same name already exists (unique constraint violation).
    /// </exception>
    Task<int> CreateAsync(ProductCreateRequestDto requestDto);

    /// <summary>
    /// Updates an existing product with the specified details.
    /// </summary>
    /// <param name="id">The unique identifier of the product to update.</param>
    /// <param name="requestDto">The request containing the product update data.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ValidationException">Thrown when the requestDto fails validation.</exception>
    /// <exception cref="NotFoundException">
    /// Thrown when the product with the specified id is not found or has been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user does not have permission to edit the product.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when one or more category IDs do not exist in the system.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict or foreign key constraint violation occurs during save.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when attempting to set a duplicate product name (unique constraint violation).
    /// </exception>
    Task UpdateAsync(int id, ProductUpdateRequestDto requestDto);

    /// <summary>
    /// Soft deletes a product by marking it as deleted.
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the product with the specified id is not found or has been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user does not have permission to delete the product.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict or foreign key constraint violation occurs during save.
    /// </exception>
    Task DeleteAsync(int id);
    #endregion
}