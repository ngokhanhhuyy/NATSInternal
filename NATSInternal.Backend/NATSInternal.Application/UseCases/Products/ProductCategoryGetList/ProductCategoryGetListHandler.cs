using FluentValidation;
using MediatR;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Products;

internal class ProductCategoryGetListHandler : IRequestHandler<ProductCategoryGetListRequestDto, ProductCategoryGetListResponseDto>
{
    #region Fields
    private readonly IProductService _service;
    private readonly IValidator<ProductCategoryGetListRequestDto> _validator;
    #endregion

    #region Constructors
    public ProductCategoryGetListHandler(IProductService service, IValidator<ProductCategoryGetListRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }
    #endregion

    #region Methods
    public async Task<ProductCategoryGetListResponseDto> Handle(
        ProductCategoryGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        return await _service.GetPaginatedProductCategoryListAsync(requestDto, cancellationToken);
    }
    #endregion
}
