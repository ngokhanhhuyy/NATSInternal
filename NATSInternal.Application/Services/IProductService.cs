using NATSInternal.Application.UseCases.Products;

namespace NATSInternal.Application.Services;

internal interface IProductService
{
    #region Methods
    Task<ProductGetListResponseDto> GetPaginatedProductListAsync(
        ProductGetListRequestDto requestDto,
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