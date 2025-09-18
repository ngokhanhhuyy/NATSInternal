using FluentValidation;
using MediatR;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Products.ProductGetList;

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
        CancellationToken cancellationToken)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        return await _service.GetPaginatedProductListAsync(
            requestDto.SortByAscending,
            requestDto.SortByFieldName,
            requestDto.Page,
            requestDto.ResultsPerPage,
            requestDto.BrandId,
            requestDto.CategoryId,
            requestDto.SearchContent,
            cancellationToken
        );
    }
    #endregion
}
