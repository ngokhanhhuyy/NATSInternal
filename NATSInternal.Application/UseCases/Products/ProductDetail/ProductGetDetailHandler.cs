using MediatR;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductGetDetailHandler : IRequestHandler<ProductGetDetailRequestDto, ProductGetDetailResponseDto>
{
    #region Fields
    private readonly IProductService _service;
    #endregion
    
    #region Constructors
    public ProductGetDetailHandler(IProductService service)
    {
        _service = service;
    }
    #endregion
    
    #region Methods
    public async Task<ProductGetDetailResponseDto> Handle(
        ProductGetDetailRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        return await _service.GetProductDetailAsync(requestDto, cancellationToken);
    }
    #endregion
}