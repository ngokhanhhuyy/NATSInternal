using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Products;
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
            Stock = _context.Stocks.First(stock => stock.ProductId == product.Id),
            ThumbnailPhoto = _context.Photos.FirstOrDefault(photo => photo.ProductId == product.Id && photo.IsThumbnail)
        });

        var productPage = await _listFetchingService.GetPagedListAsync(
            projectedQuery,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        List<ProductBasicResponseDto> productResponseDtos = productPage.Items
            .Select(i => new ProductBasicResponseDto(
                product: i.Product,
                stock: i.Stock,
                thumbnailPhoto: i.ThumbnailPhoto,
                _authorizationInternalService.GetProductExistingAuthorization(i.Product)))
            .ToList();

        return new(productResponseDtos, productPage.PageCount);
    }

    public async Task<ProductGetDetailResponseDto> GetProductDetailAsync(
        ProductGetDetailRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        var result = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.Id == requestDto.Id && !p.IsDeleted)
            .Select(product => new
            {
                Product = product,
                Stock = _context.Stocks.First(stock => stock.ProductId == product.Id),
                Photos = _context.Photos.Where(photo => photo.ProductId == product.Id).ToList(),
                CreatedUser = _context.Users.First(user => user.Id == product.CreatedUserId),
                LastUpdatedUser = _context.Users.FirstOrDefault(user => user.Id == product.LastUpdatedUserId)
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException();

        return new(
            product: result.Product,
            stock: result.Stock,
            createdUser: result.CreatedUser,
            lastUpdatedUser: result.LastUpdatedUser,
            photos: result.Photos,
            authorizationResponseDto: _authorizationInternalService.GetProductExistingAuthorization(result.Product)
        );
    }
    #endregion
}