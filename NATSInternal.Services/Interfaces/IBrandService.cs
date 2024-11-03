namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to perform the brand-related operations.
/// </summary>
public interface IBrandService
{
    /// <summary>
    /// Retrives a list which contains the basic information of brands based on the sorting
    /// and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// (Optional) A DTO containing the conditions for the results.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="BrandListResponseDto"/> class, containing the results.
    /// </returns>
    Task<BrandListResponseDto> GetListAsync(BrandListRequestDto requestDto);

    /// <summary>
    /// Retrives a list of all brands with minimal information.
    /// </summary>
    /// <returns>
    /// A list of DTO, containing the minimal information of the all brands.
    /// </returns>
    Task<List<BrandMinimalResponseDto>> GetAllAsync();

    /// <summary>
    /// Retrives the details of a specific brand.
    /// </summary>
    /// <param name="id">
    /// A <see cref="int"/> represening the id of the brand.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// instance of the <see cref="BrandDetailResponseDto"/>, containing the details of the
    /// brand.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the brand with the specified id doesn't exist or has already been deleted.
    /// </exception>
    Task<BrandDetailResponseDto> GetDetailAsync(int id);

    /// <summary>
    /// Create a new brand with the data provided in the request.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="BrandUpsertRequestDto"/>, containing the data for the new
    /// brand.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// <see cref="int"/> representing the id of the new brand.
    /// </returns>
    /// <exception cref="OperationException">
    /// Throws when the country with the id specified by the value of the property
    /// <c>Country.Id</c> in the <paramref name="requestDto"/> argument doesn't
    /// exist or when the name specified by the value of the property <c>Name</c> in the
    /// <paramref name="requestDto"/> argument already exists.
    /// </exception>
    Task<int> CreateAsync(BrandUpsertRequestDto requestDto);

    /// <summary>
    /// Update an existing brand.
    /// </summary>
    /// <param name="id">
    /// A <see cref="int"/> representing the id of the brand to be updated.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="BrandUpsertRequestDto"/>, containing the data for the brand
    /// to be updated.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the brand with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the country with the id specified by the value of the property
    /// <c>Country.Id</c> doesn't exist or when the specified name by the property <c>Name</c>
    /// in the <paramref name="requestDto"/> argument already exists.
    /// </exception>
    Task UpdateAsync(int id, BrandUpsertRequestDto requestDto);

    /// <summary>
    /// Delete an existing brand.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the brand to be deleted.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the brand with the specified id doesn't exist or has already been deleted.
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
    /// Check if the requesting user has permission to create a new brand.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();
}
