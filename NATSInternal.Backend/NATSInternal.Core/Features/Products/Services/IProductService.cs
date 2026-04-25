namespace NATSInternal.Core.Features.Products;

public interface IProductService
{
    #region Properties
    Task<ProductListResponseDto> GetListAsync(ProductListRequestDto requestDto);
    Task<ProductDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(ProductCreateRequestDto requestDto);
    Task UpdateAsync(int id, ProductUpdateRequestDto requestDto);
    Task DeleteAsync(int id);
    #endregion
}