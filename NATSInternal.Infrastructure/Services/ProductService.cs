using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Products;
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
    private readonly IAuthorizationService _authorizationService;
    #endregion

    #region Constructors
    public ProductService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationService authorizationService)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationService = authorizationService;
    }
    #endregion

    #region Methods
    public async Task<ProductGetListResponseDto> GetPaginatedProductListAsync(
        ProductGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category);

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

        bool sortByAscendingOrDefault = requestDto.SortByAscending ?? true;
        switch (requestDto.SortByFieldName)
        {
            case nameof(Product.CreatedDateTime) or null:
                query = query.ApplySorting(p => p.CreatedDateTime, sortByAscendingOrDefault);
                break;
            case nameof(Stock.StockingQuantity):
                query = query.ApplySorting(
                    p => EF.Property<int>(p, nameof(Stock.StockingQuantity)),
                    sortByAscendingOrDefault);
                break;
            default:
                throw new NotImplementedException();
        }

        var projectedQuery = query.Select(product => new
        {
            Product = product,
            StockingQuantity = _context.Stocks
                .Where(stock => stock.ProductId == product.Id)
                .Select(stock => stock.StockingQuantity)
                .First(),
            ThumbnailUrl = _context.Photos
                .Where(photo => photo.ProductId == product.Id && photo.IsThumbnail)
                .Select(photo => (string?)photo.Url)
                .FirstOrDefault()
        });

        var productPage = await _listFetchingService.GetPagedListAsync(
            projectedQuery,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        List<ProductGetListProductResponseDto> productResponseDtos = productPage.Items
            .Select(i => new ProductGetListProductResponseDto(
                i.Product,
                i.StockingQuantity,
                i.ThumbnailUrl,
                _authorizationService.GetProductExistingAuthorization(i.Product)))
            .ToList();

        return new(productResponseDtos, productPage.PageCount);
    }
    #endregion
}