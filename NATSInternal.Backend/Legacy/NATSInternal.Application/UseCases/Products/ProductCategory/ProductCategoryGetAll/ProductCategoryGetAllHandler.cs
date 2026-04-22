using MediatR;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductCategoryGetAllHandler
    : IRequestHandler<ProductCategoryGetAllRequestDto, IEnumerable<ProductCategoryBasicResponseDto>>
{
    #region Fields
    private readonly IProductService _service;
    #endregion

    #region Constructors
    public ProductCategoryGetAllHandler(IProductService service)
    {
        _service = service;
    }
    #endregion

    #region Methods
    public async Task<IEnumerable<ProductCategoryBasicResponseDto>> Handle(
        ProductCategoryGetAllRequestDto _,
        CancellationToken cancellationToken)
    {
        return await _service.GetAllProductCategoriesAsync(cancellationToken);
    }
    #endregion
}