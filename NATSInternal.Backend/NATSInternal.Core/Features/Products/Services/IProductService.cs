using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Products;

public interface IProductService
{
    #region Properties
    Task<ProductListResponseDto> GetListAsync(ProductListRequestDto requestDto);
    Task<List<ProductMinimalResponseDto>> GetAllAsync();
    Task<ProductDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(ProductCreateRequestDto requestDto);
    Task UpdateAsync(int id, ProductUpdateRequestDto requestDto);
    Task DeleteAsync(int id);
    Task<TopOverTimeRangeResponseDto<ProductBasicResponseDto, int>> GetTopBySoldQuantity(TopRequestDto requestDto);
    Task<TopOverTimeRangeResponseDto<ProductBasicResponseDto, long>> GetTopByRevenue(TopRequestDto requestDto);
    #endregion
}
