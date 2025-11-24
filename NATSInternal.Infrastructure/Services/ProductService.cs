using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Extensions;

namespace NATSInternal.Infrastructure.Services;

internal class ProductService : IProductService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationInternalService;
    #endregion

    #region Constructors
    public ProductService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationInternalService)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationInternalService = authorizationInternalService;
    }
    #endregion

    #region Methods
    public async Task<ProductGetListResponseDto> GetPaginatedProductListAsync(
        ProductGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.DeletedDateTime == null);

        if (requestDto.BrandId.HasValue)
        {
            query = query.Where(p => p.BrandId == requestDto.BrandId.Value);
        }

        if (requestDto.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == requestDto.CategoryId.Value);
        }

        if (requestDto.SearchContent is not null && requestDto.SearchContent.Length > 0)
        {
            string searchContent = requestDto.SearchContent;
            query = query.Where(p =>
                EF.Functions.Like(p.Name.ToLower(), $"%{searchContent.ToLower()}%") ||
                p.Brand != null && EF.Functions.Like(p.Brand.Name.ToLower(), $"%{searchContent.ToLower()}%") ||
                p.Category != null && EF.Functions.Like(p.Category.Name.ToLower(), $"%{searchContent.ToLower()}%")
            );
        }

        var projectedQuery = query.Select(product => new ProductWithStockAndThumbnail
        {
            Product = product,
            Stock = _context.Stocks.First(stock => stock.ProductId == product.Id),
            Thumbnail = _context.Photos.FirstOrDefault(photo => photo.ProductId == product.Id && photo.IsThumbnail)
        });

        switch (requestDto.SortByFieldName)
        {
            case nameof(ProductGetListRequestDto.FieldToSort.Name):
                projectedQuery = projectedQuery.ApplySorting(pst => pst.Product.Name, requestDto.SortByAscending);
                break;
            case nameof(ProductGetListRequestDto.FieldToSort.DefaultAmountBeforeVatPerUnit):
                projectedQuery = projectedQuery.ApplySorting(
                    pst => pst.Product.DefaultAmountBeforeVatPerUnit,
                    requestDto.SortByAscending
                );
                break;
            case nameof(ProductGetListRequestDto.FieldToSort.CreatedDateTime):
                projectedQuery = projectedQuery.ApplySorting(
                    pst => pst.Product.CreatedDateTime,
                    requestDto.SortByAscending
                );
                break;
            case nameof(ProductGetListRequestDto.FieldToSort.StockingQuantity):
                projectedQuery = projectedQuery.ApplySorting(
                    pst => pst.Stock != null ? pst.Stock.StockingQuantity : 0,
                    requestDto.SortByAscending
                );
                break;
            default:
                throw new NotImplementedException();
        }

        Page<ProductWithStockAndThumbnail> queryResult = await _listFetchingService.GetPagedListAsync(
            projectedQuery,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        List<ProductGetListProductResponseDto> productResponseDtos = queryResult.Items
            .Select(pst => new ProductGetListProductResponseDto(
                pst.Product,
                pst.Stock,
                pst.Thumbnail,
                _authorizationInternalService.GetProductExistingAuthorization(pst.Product)))
            .ToList();

        return new(productResponseDtos, queryResult.PageCount);
    }
    #endregion
}

file class ProductWithStockAndThumbnail
{
    #region Properties
    public required Product Product { get; init; }
    public required Stock? Stock { get; init; }
    public required Photo? Thumbnail { get; init; }
    #endregion
}