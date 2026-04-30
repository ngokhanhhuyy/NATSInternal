namespace NATSInternal.Core.Features.Products;

/// <summary>
/// Defines operations for retrieving, creating, updating, and deleting product categories.
/// </summary>
public interface IProductCategoryService
{
    #region Methods
    /// <summary>
    /// Retrieves all product categories ordered by name.
    /// </summary>
    /// <returns>
    /// A task that returns the complete list of product category summaries.
    /// </returns>
    /// <example>
    /// <code>
    /// List&lt;ProductCategoryBasicResponseDto&gt; categories = await productCategoryService.GetAllAsync();
    /// </code>
    /// </example>
    Task<List<ProductCategoryBasicResponseDto>> GetAllAsync();

    /// <summary>
    /// Retrieves detailed information for a product category by identifier.
    /// </summary>
    /// <param name="id">The identifier of the product category to retrieve.</param>
    /// <returns>
    /// A task that returns the detailed product category response for the matching category.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no product category exists for <paramref name="id"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// ProductCategoryDetailResponseDto category = await productCategoryService.GetDetailAsync(4);
    /// </code>
    /// </example>
    Task<ProductCategoryDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new product category from the supplied data and returns its identifier.
    /// </summary>
    /// <param name="requestDto">
    /// The request containing the product category data to persist.
    /// </param>
    /// <returns>
    /// A task that returns the identifier assigned to the created product category.
    /// </returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when <paramref name="requestDto"/> fails validation.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to create product categories.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict occurs while saving the new product category.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when another product category already uses the supplied name.
    /// </exception>
    /// <example>
    /// <code>
    /// int categoryId = await productCategoryService.CreateAsync(new ProductCategoryUpsertRequestDto
    /// {
    ///     Name = "Skincare"
    /// });
    /// </code>
    /// </example>
    Task<int> CreateAsync(ProductCategoryUpsertRequestDto requestDto);

    /// <summary>
    /// Updates an existing product category with the supplied data.
    /// </summary>
    /// <param name="id">The identifier of the product category to update.</param>
    /// <param name="requestDto">
    /// The request containing the updated product category data.
    /// </param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when <paramref name="requestDto"/> fails validation.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Thrown when no product category exists for <paramref name="id"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to edit the specified product category.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict occurs while saving the product category changes.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when another product category already uses the supplied name.
    /// </exception>
    /// <example>
    /// <code>
    /// await productCategoryService.UpdateAsync(4, new ProductCategoryUpsertRequestDto
    /// {
    ///     Name = "Hair Care"
    /// });
    /// </code>
    /// </example>
    Task UpdateAsync(int id, ProductCategoryUpsertRequestDto requestDto);

    /// <summary>
    /// Deletes an existing product category.
    /// </summary>
    /// <param name="id">The identifier of the product category to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no product category exists for <paramref name="id"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to delete the specified product category.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict occurs while saving the delete operation.
    /// </exception>
    /// <example>
    /// <code>
    /// await productCategoryService.DeleteAsync(4);
    /// </code>
    /// </example>
    Task DeleteAsync(int id);
    #endregion
}
   