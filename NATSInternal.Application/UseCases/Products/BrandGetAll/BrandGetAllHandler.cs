using MediatR;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Products;

internal class BrandGetAllHandler : IRequestHandler<BrandGetAllRequestDto, IEnumerable<BrandBasicResponseDto>>
{
    #region Fields
    private readonly IProductService _service;
    #endregion

    #region Constructors
    public BrandGetAllHandler(IProductService service)
    {
        _service = service;
    }
    #endregion

    #region Methods
    public async Task<IEnumerable<BrandBasicResponseDto>> Handle(
        BrandGetAllRequestDto _,
        CancellationToken cancellationToken)
    {
        return await _service.GetAllBrandsAsync(cancellationToken);
    }
    #endregion
}