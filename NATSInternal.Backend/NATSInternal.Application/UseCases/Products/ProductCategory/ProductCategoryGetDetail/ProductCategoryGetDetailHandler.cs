using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductCategoryGetDetailHandler
    : IRequestHandler<ProductCategoryGetDetailRequestDto, ProductCategoryGetDetailResponseDto>
{
    #region Fields
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    #endregion

    #region Constructors
    public ProductCategoryGetDetailHandler(IProductRepository productCategoryRepository, IUserRepository userRepository)
    {
        _productRepository = productCategoryRepository;
        _userRepository = userRepository;
    }
    #endregion

    #region Methods
    public async Task<ProductCategoryGetDetailResponseDto> Handle(
        ProductCategoryGetDetailRequestDto requestDto,
        CancellationToken token)
    {
        ProductCategory brand = await _productRepository.GetCategoryByIdAsync(requestDto.Id, token)
            ?? throw new NotFoundException();

        User? createdUser = await _userRepository.GetUserByIdAsync(brand.CreatedUserId, token);

        if (brand.LastUpdatedUserId.HasValue)
        {
            User? lastUpdatedUser = await _userRepository.GetUserByIdAsync(brand.LastUpdatedUserId.Value, token);
            return new(brand, createdUser, lastUpdatedUser);
        }

        return new(brand, createdUser);
    }
    #endregion
}