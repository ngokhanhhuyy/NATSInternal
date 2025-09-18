using NATSInternal.Application.UseCases.Products;

namespace NATSInternal.Application.Services;

internal interface IProductService
{
    #region Methods
    Task<ProductGetListResponseDto> GetPaginatedProductListAsync(
        bool? sortByAscending,
        string? sortByFieldName,
        int? page,
        int? resultsPerPage,
        Guid? brandId,
        Guid? categoryId,
        string? searchContent,
        CancellationToken cancellationToken = default);

    // Task<Page<Brand>> GetBrandListAsync(
    //     bool? sortByAscending,
    //     string? sortByFieldName,
    //     int? page,
    //     int? resultsPerPage,
    //     Guid? roleId,
    //     string? searchContent,
    //     CancellationToken cancellationToken = default);
    #endregion
}