using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Shared;
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
    #endregion

    #region Constructors
    public ProductService(AppDbContext context, IListFetchingService listFetchingService)
    {
        _context = context;
        _listFetchingService = listFetchingService;
    }
    #endregion

    #region Methods
    public async Task<Page<(Product, int?, string?)>> GetProductPagedListIncludingBrandAndCategoryAsync(
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

        bool sortByAscendingOrDefault = sortByAscending ?? true;
        switch (sortByFieldName)
        {
            case nameof(Product.CreatedDateTime) or null:
                query = query.ApplySorting(p => p.CreatedDateTime, sortByAscendingOrDefault);
                break;
            case nameof(Stock.StockingQuantity):
                query = query.ApplySorting(p => EF.Property<int>(p, "stocking_quantity"), sortByAscendingOrDefault);
                break;
            default:
                throw new NotImplementedException();
        }

        IQueryable<(Product, int?, string?)> projectedQuery = query.Select(product =>
        (
            product,
            _context.Stocks
                .Where(stock => stock.ProductId == product.Id)
                .Select(stock => (int?)stock.StockingQuantity)
                .FirstOrDefault(),
            _context.Photos
                .Where(photo => photo.ProductId == product.Id && photo.IsThumbnail)
                .Select(photo => (string?)photo.Url)
                .FirstOrDefault()
        ));

        Page<Product> productPage = await _listFetchingService.GetPagedListAsync(
            projectedQuery,
            page,
            resultsPerPage,
            cancellationToken
        );
        
        return 
    }
    #endregion
}