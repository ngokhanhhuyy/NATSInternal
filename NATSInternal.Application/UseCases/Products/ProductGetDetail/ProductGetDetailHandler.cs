using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductGetDetailHandler : IRequestHandler<ProductGetDetailRequestDto, ProductGetDetailResponseDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    private readonly IAuthorizationInternalService _authorizationService;
    #endregion
    
    #region Constructors
    public ProductGetDetailHandler(IProductRepository repository, IAuthorizationInternalService authorizationService)
    {
        _repository = repository;
        _authorizationService = authorizationService;
    }
    #endregion
    
    #region Methods
    public async Task<ProductGetDetailResponseDto> Handle(
        ProductGetDetailRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        Product product = await _repository
            .GetProductByIdIncludingBrandWithCountryAndCategoryAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();
        
        return new(product, _authorizationService.GetProductExistingAuthorization(product));
    }
    #endregion
}