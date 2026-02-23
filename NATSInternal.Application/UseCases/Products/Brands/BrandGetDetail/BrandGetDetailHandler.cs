using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

internal class BrandGetDetailHandler : IRequestHandler<BrandGetDetailRequestDto, BrandGetDetailResponseDto>
{
    #region Fields
    private readonly IProductRepository _repository;
    #endregion

    #region Constructors
    public BrandGetDetailHandler(IProductRepository repository)
    {
        _repository = repository;
    }
    #endregion

    #region Methods
    public async Task<BrandGetDetailResponseDto> Handle(BrandGetDetailRequestDto requestDto, CancellationToken token)
    {
        Brand brand = await _repository.GetBrandByIdAsync(requestDto.Id, token)
            ?? throw new NotFoundException();

        return new(brand);
    }
    #endregion
}