using FluentValidation;
using MediatR;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Products;

internal class BrandGetListHandler : IRequestHandler<BrandGetListRequestDto, BrandGetListResponseDto>
{
    #region Fields
    private readonly IProductService _service;
    private readonly IValidator<BrandGetListRequestDto> _validator;
    #endregion

    #region Constructors
    public BrandGetListHandler(IProductService service, IValidator<BrandGetListRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }
    #endregion

    #region Methods
    public async Task<BrandGetListResponseDto> Handle(
        BrandGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        return await _service.GetPaginatedBrandListAsync(requestDto, cancellationToken);
    }
    #endregion
}
