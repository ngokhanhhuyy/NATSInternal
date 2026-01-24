using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductGetDetailHandler : IRequestHandler<ProductGetDetailRequestDto, ProductGetDetailResponseDto>
{
    #region Fields
    private readonly IPhotoRepository _photoRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationInternalService _authorizationService;
    #endregion
    
    #region Constructors
    public ProductGetDetailHandler(
        IPhotoRepository photoRepository,
        IProductRepository productRepository,
        IStockRepository stockRepository,
        IUserRepository userRepository,
        IAuthorizationInternalService authorizationService)
    {
        _photoRepository = photoRepository;
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _userRepository = userRepository;
        _authorizationService = authorizationService;
    }
    #endregion
    
    #region Methods
    public async Task<ProductGetDetailResponseDto> Handle(
        ProductGetDetailRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        Product product = await _productRepository
            .GetProductByIdIncludingBrandWithCountryAndCategoryAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        Stock? stock = await _stockRepository.GetSingleStockByProductIdAsync(product.Id, cancellationToken);

        ICollection<Photo> photos = await _photoRepository.GetMultiplePhotosByProductIdsAsync(
            [product.Id],
            cancellationToken
        );

        User? createdUser = await _userRepository.GetUserByIdAsync(product.CreatedUserId, cancellationToken);
        if (product.LastUpdatedUserId is null)
        {
            return new(product, createdUser, photos, _authorizationService.GetProductExistingAuthorization(product));
        }

        User? lastUpdatedUser = await _userRepository.GetUserByIdAsync(
            product.LastUpdatedUserId.Value,
            cancellationToken
        );
        
        return new(
            product,
            createdUser,
            lastUpdatedUser,
            photos,
            _authorizationService.GetProductExistingAuthorization(product)
        );

    }
    #endregion
}