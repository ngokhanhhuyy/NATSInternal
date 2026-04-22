using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Products;

internal class BrandGetDetailHandler : IRequestHandler<BrandGetDetailRequestDto, BrandGetDetailResponseDto>
{
    #region Fields
    private readonly IProductRepository _brandRepository;
    private readonly IUserRepository _userRepository;
    #endregion

    #region Constructors
    public BrandGetDetailHandler(IProductRepository brandRepository, IUserRepository userRepository)
    {
        _brandRepository = brandRepository;
        _userRepository = userRepository;
    }
    #endregion

    #region Methods
    public async Task<BrandGetDetailResponseDto> Handle(BrandGetDetailRequestDto requestDto, CancellationToken token)
    {
        Brand brand = await _brandRepository.GetBrandByIdIncludingCountryAsync(requestDto.Id, token)
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