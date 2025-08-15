namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle the product-category-related operations.
/// </summary>
public interface IProductCategoryService
{
    /// <summary>
    /// Retrieves a list of product categories, based on paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the paginating conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// an instance of the <see cref="ProductCategoryListResponseDto"/> class, containing the
    /// results.
    /// </returns>
    Task<ProductCategoryListResponseDto> GetListAsync(
            ProductCategoryListRequestDto requestDto);

    /// <summary>
    /// Retrieves a list of all product categories with minimal information.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// a <see cref="List{T}"/> of the <see cref="ProductCategoryMinimalResponseDto"/> DTOs,
    /// containing results.
    /// </returns>
    Task<List<ProductCategoryMinimalResponseDto>> GetAllAsync();

    /// <summary>
    /// Retrieves the details a specfic product category, based on its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the product category to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// an instance of the <see cref="ProductCategoryResponseDto"/> class, containing the
    /// details of the product category.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the product category with the specified id doesn't exist or has already
    /// been deleted.
    /// </exception>
    Task<ProductCategoryResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Creates a new product category based the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="ProductCategoryRequestDto"/> class, containing the data
    /// for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is
    /// an <see cref="int"/> representing the id of the new product category.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the unique value for the <c>Name</c> property in the <c>requestDto</c>
    /// argument already exists.
    /// </exception>
    Task<int> CreateAsyns(ProductCategoryRequestDto requestDto);

    /// <summary>
    /// Updates an existing product category based on its id and the specified data.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the product category to update.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="ProductCategoryRequestDto"/> class, contanining the data
    /// for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the product category with the specfied id doens't exist or has already been
    /// deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the unique value for the <c>Name</c> property in the <c>requestDto</c>
    /// argument already exists.
    /// </exception>
    Task UpdateAsync(int id, ProductCategoryRequestDto requestDto);

    /// <summary>
    /// Deletes an existing product category, based on its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the product category to delete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the product category with the specified id doesn't exist or has already
    /// been deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the product category's deletion is restricted due to the existence of some
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
    /// Check if the requesting user has permission to create a new product category.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();
}
