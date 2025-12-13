using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.Services;

internal interface IProductService
{
    #region Methods
    Task<ProductGetListResponseDto> GetPaginatedProductListAsync(
        ProductGetListRequestDto requestDto,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<BrandBasicResponseDto>> GetAllBrandsAsync(CancellationToken cancellationToken = default);

    Task<BrandGetListResponseDto> GetPaginatedBrandListAsync(
        BrandGetListRequestDto requestDto,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<ProductCategoryBasicResponseDto>> GetAllProductCategoriesAsync(
        CancellationToken cancellationToken = default);

    Task<ProductCategoryGetListResponseDto> GetPaginatedProductCategoryListAsync(
        ProductCategoryGetListRequestDto requestDto,
        CancellationToken cancellationToken = default);
    #endregion
}