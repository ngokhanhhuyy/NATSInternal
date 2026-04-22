using FluentValidation;
using MediatR;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductGetListHandler : IRequestHandler<ProductGetListRequestDto, ProductGetListResponseDto>
{
    #region Fields
    private readonly IProductService _service;
    private readonly IValidator<ProductGetListRequestDto> _validator;
    #endregion

    #region Constructors
    public ProductGetListHandler(
        IProductService service,
        IValidator<ProductGetListRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }
    #endregion

    #region Methods
    public async Task<ProductGetListResponseDto> Handle(
        ProductGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        return await _service.GetPaginatedProductListAsync(requestDto, cancellationToken);
    }
    #endregion
}
