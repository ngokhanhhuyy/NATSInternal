namespace NATSInternal.Core.Features.Products;

public interface IProductCategoryService
{
    #region Methods
    Task<ProductCategoryBasicResponseDto> GetAllAysnc();
    Task<ProductCategoryDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(ProductCategoryUpsertRequestDto requestDto);
    Task UpdateAsync(int id, ProductCategoryUpsertRequestDto requestDto);
    Task DeleteAsync(int id);
    #endregion
}
