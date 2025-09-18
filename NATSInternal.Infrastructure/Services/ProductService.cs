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
        bool? sortByAscending,
        string? sortByFieldName,
        int? page,
        int? resultsPerPage,
        Guid? brandId,
        Guid? categoryId,
        string? searchContent,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category);

        if (brandId.HasValue)
        {
            query = query.Where(p => p.BrandId == brandId.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (searchContent is not null && searchContent.Length > 0)
        {
            query = query.Where(p =>
                EF.Functions.Like(p.Name.ToLower(), $"%{searchContent.ToLower()}%") ||
                p.Brand != null && EF.Functions.Like(p.Brand.Name.ToLower(), $"%{searchContent.ToLower()}%") ||
                p.Category != null && EF.Functions.Like(p.Category.Name.ToLower(), $"%{searchContent.ToLower()}%")
            );
        }

        bool sortByAscendingOrDefault = sortByAscending ?? true;
        switch (sortByFieldName)
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
            page,
            resultsPerPage,
            cancellationToken
        );

        List<ProductGetListProductResponseDto> productResponseDtos = productPage.Items
            .Select(i => new ProductGetListProductResponseDto(i.Product, i.StockingQuantity, i.ThumbnailUrl))
            .ToList();

        return new(productResponseDtos, productPage.PageCount);
    }
    #endregion
}