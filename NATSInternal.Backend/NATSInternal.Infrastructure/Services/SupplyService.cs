using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Supplies;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Supplies;
using NATSInternal.Domain.Features.Stocks;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Extensions;

namespace NATSInternal.Infrastructure.Services;

internal class SupplyService : ISupplyService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationInternalService;
    #endregion

    #region Constructors
    public SupplyService(
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
    public async Task<SupplyGetListResponseDto> GetPaginatedSupplyListAsync(
        SupplyGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Supply> query = _context.Supplies
            .Include(p => p.Items)
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

        var projectedQuery = query.Select(product => new SupplyWithStockAndThumbnail
        {
            Supply = product,
            Stock = _context.Stocks.First(stock => stock.SupplyId == product.Id),
            Thumbnail = _context.Photos.FirstOrDefault(photo => photo.SupplyId == product.Id && photo.IsThumbnail)
        });

        switch (requestDto.SortByFieldName)
        {
            case nameof(SupplyGetListRequestDto.FieldToSort.Status):
                projectedQuery = projectedQuery
                    .ApplySorting(pst => pst.Supply.IsDiscontinued, requestDto.SortByAscending)
                    .ThenApplySorting(
                        pst => pst.Stock != null
                            ? pst.Stock.StockingQuantity - pst.Stock.ResupplyThresholdQuantity
                            : int.MaxValue,
                        requestDto.SortByAscending)
                    .ThenApplySorting(pst => pst.Supply.Name, requestDto.SortByAscending);
                break;
            case nameof(SupplyGetListRequestDto.FieldToSort.Name):
                projectedQuery = projectedQuery.ApplySorting(pst => pst.Supply.Name, requestDto.SortByAscending);
                break;
            case nameof(SupplyGetListRequestDto.FieldToSort.DefaultAmountBeforeVatPerUnit):
                projectedQuery = projectedQuery.ApplySorting(
                    pst => pst.Supply.DefaultAmountBeforeVatPerUnit,
                    requestDto.SortByAscending
                );
                break;
            case nameof(SupplyGetListRequestDto.FieldToSort.CreatedDateTime):
                projectedQuery = projectedQuery.ApplySorting(
                    pst => pst.Supply.CreatedDateTime,
                    requestDto.SortByAscending
                );
                break;
            case nameof(SupplyGetListRequestDto.FieldToSort.StockingQuantity):
                projectedQuery = projectedQuery.ApplySorting(
                    pst => pst.Stock != null ? pst.Stock.StockingQuantity : 0,
                    requestDto.SortByAscending
                );
                break;
            default:
                throw new NotImplementedException();
        }

        Page<SupplyWithStockAndThumbnail> queryResult = await _listFetchingService.GetPagedListAsync(
            projectedQuery,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        List<SupplyGetListSupplyResponseDto> productResponseDtos = queryResult.Items
            .Select(pst => new SupplyGetListSupplyResponseDto(
                pst.Supply,
                pst.Stock,
                pst.Thumbnail,
                _authorizationInternalService.GetSupplyExistingAuthorization(pst.Supply)))
            .ToList();

        return new(productResponseDtos, queryResult.PageCount, queryResult.ItemCount);
    }

    public async Task<IEnumerable<BrandBasicResponseDto>> GetAllBrandsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Brands
            .OrderBy(b => b.Name)
            .Select(b => new BrandBasicResponseDto(b))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<BrandGetListResponseDto> GetPaginatedBrandListAsync(
        BrandGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Brand> query = _context.Brands.Include(b => b.Country);

        if (requestDto.SearchContent is not null && requestDto.SearchContent.Length > 0)
        {
            query = query.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{requestDto.SearchContent.ToLower()}%"));
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(BrandGetListRequestDto.FieldToSort.Name):
                query = query.ApplySorting(b => b.Name, requestDto.SortByAscending);
                break;
            case nameof(BrandGetListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(b => b.CreatedDateTime, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        IQueryable<BrandWithSupplyCount> projectedQuery = query.Select(b => new BrandWithSupplyCount
        {
            Brand = b,
            SupplyCount = _context.Supplys.Count(p => p.BrandId == b.Id)
        });

        Page<BrandWithSupplyCount> queryResult = await _listFetchingService.GetPagedListAsync(
            projectedQuery,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        IEnumerable<BrandGetListBrandResponseDto> brandResponseDtos = queryResult.Items
            .Select(i =>
            {
                BrandExistingAuthorizationResponseDto authorizationResponseDto;
                authorizationResponseDto = _authorizationInternalService.GetBrandExistingAuthorization(i.Brand);
                return new BrandGetListBrandResponseDto(i.Brand, i.SupplyCount, authorizationResponseDto);
            });

        bool canCreate = _authorizationInternalService.CanCreateBrand();
        return new(brandResponseDtos, queryResult.PageCount, queryResult.ItemCount, canCreate);
    }

    public async Task<IEnumerable<SupplyCategoryBasicResponseDto>> GetAllSupplyCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SupplyCategories
            .OrderBy(pc => pc.Name)
            .Select(pc => new SupplyCategoryBasicResponseDto(pc))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<SupplyCategoryGetListResponseDto> GetPaginatedSupplyCategoryListAsync(
        SupplyCategoryGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<SupplyCategory> query = _context.SupplyCategories;

        if (requestDto.SearchContent is not null && requestDto.SearchContent.Length > 0)
        {
            query = query.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{requestDto.SearchContent.ToLower()}%"));
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(BrandGetListRequestDto.FieldToSort.Name):
                query = query.ApplySorting(b => b.Name, requestDto.SortByAscending);
                break;
            case nameof(BrandGetListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(b => b.CreatedDateTime, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        Page<SupplyCategory> queryResult = await _listFetchingService.GetPagedListAsync(
            query,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        IEnumerable<SupplyCategoryGetListSupplyCategoryResponseDto> categoryResponseDtos = queryResult.Items
            .Select(pc =>
            {
                SupplyCategoryExistingAuthorizationResponseDto authorizationResponseDto;
                authorizationResponseDto = _authorizationInternalService.GetSupplyCategoryExistingAuthorization(pc);
                return new SupplyCategoryGetListSupplyCategoryResponseDto(pc, authorizationResponseDto);
            });

        bool canCreate = _authorizationInternalService.CanCreateSupplyCategory();
        return new(categoryResponseDtos, queryResult.PageCount, queryResult.ItemCount, canCreate);
    }
    #endregion
}