namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle the product-related operations.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieve a list of products with the basic information, based on the specified filtering, sorting and paginating
    /// conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="ProductListRequestDto"/> class, containing the conditions for the results.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an instance of the
    /// <see cref="ProductListResponseDto"/> class, containing the results and the additional information for
    /// pagination.
    /// </returns>
    Task<ProductListResponseDto> GetListAsync(
            ProductListRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the details of a specific product, specified by its id.
    /// </summary>
    /// <param name="id">
    /// A <see cref="Guid"/> value representing the id of the product to retrieve.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an instance of the
    /// <see cref="ProductDetailResponseDto"/> class, containing the details of the product.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the product with the specified <c>id</c> doesn't exist or has already been deleted.
    /// </exception>
    Task<ProductDetailResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new product based on the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="ProductUpsertRequestDto"/> class, contanining the data for the operation.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an <see cref="int"/> representing
    /// the id of the new product.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the brand with the id specified by the value of the property <c>BrandId</c> in the
    /// <paramref name="requestDto"/> doesn't exist or has already been deleted.<br/>
    /// - When the category with the id specified by the value of the property <c>CategoryId</c> in the
    /// <paramref name="requestDto"/> doens't exist or has already been deleted.<br/>
    /// - When the specfied value for the property <c>Name</c> in the <c>requestDto</c> argument already exists.
    /// </exception>
    Task<Guid> CreateAsync(ProductUpsertRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product based on its id and the specified data.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the product to update.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="ProductUpsertRequestDto"/> class, containing the data for the operation.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the product with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the brand with the id specified by the value of the property <c>BrandId</c> in the
    /// <paramref name="requestDto"/> doesn't exist or has already been deleted.<br/>
    /// - When the category with the id specified by the value of the property <c>CategoryId</c> in the
    /// <paramref name="requestDto"/> doens't exist or has already been deleted.<br/>
    /// - When the specfied value for the property <c>Name</c> property in the <paramref name="requestDto"/> already
    /// exists.
    /// </exception>
    Task UpdateAsync(Guid id, ProductUpsertRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing product based on its id.
    /// </summary>
    /// <param name="id">
    /// A <see cref="Guid"/> representing the id of the product to delete.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the product with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the product's deletion is restricted due to the existence of some related data.
    /// </exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all fields those are used as options to order the results in list retrieving operation.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="ListSortingOptionsResponseDto"/> DTO, containing the options with name and display
    /// names of the fields and the default field.
    /// </returns>
    ListSortingOptionsResponseDto GetListSortingOptions();

    /// <summary>
    /// Check if the requesting user has permission to create a new product.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();
}
