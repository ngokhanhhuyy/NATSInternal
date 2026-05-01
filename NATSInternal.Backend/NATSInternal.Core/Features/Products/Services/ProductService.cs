using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Authorization;
// using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;

namespace NATSInternal.Core.Features.Products;

internal class ProductService : IProductService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IValidator<ProductListRequestDto> _listValidator;
    private readonly IValidator<ProductCreateRequestDto> _createValidator;
    private readonly IValidator<ProductUpdateRequestDto> _updateValidator;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public ProductService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationService,
        IValidator<ProductListRequestDto> listValidator,
        IValidator<ProductCreateRequestDto> createValidator,
        IValidator<ProductUpdateRequestDto> updateValidator,
        IDbExceptionHandler exceptionHandler,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationService = authorizationService;
        _listValidator = listValidator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _exceptionHandler = exceptionHandler;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task<ProductListResponseDto> GetListAsync(ProductListRequestDto requestDto)
    {
        _listValidator.ValidateAndThrow(requestDto);
        
        IQueryable<Product> query = _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Stock)
            .Include(p => p.Photos.Where(photo => photo.IsThumbnail))
            .Where(c => c.DeletedDateTime == null);

        if (!string.IsNullOrEmpty(requestDto.SearchContent))
        {
            string lowercasedSearchContent = requestDto.SearchContent.ToLower();
            query = query.Where(c => (
                c.Name.ToLower().Contains(lowercasedSearchContent) ||
                (c.Description != null && c.Description.Contains(lowercasedSearchContent))
            ));
        }

        if (requestDto.CategoryIds.Count > 0)
        {
            query = query.Where(p => p.Categories.Select(pc => pc.Id).Any(pci => requestDto.CategoryIds.Contains(pci)));
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(ProductListRequestDto.FieldToSort.Status):
                query = query
                    .ApplySorting(p => p.IsDiscontinued, requestDto.SortByAscending)
                    .ThenApplySorting(
                        p => p.Stock != null
                            ? p.Stock.StockingQuantity - p.Stock.ResupplyThresholdQuantity
                            : int.MaxValue,
                        requestDto.SortByAscending)
                    .ThenApplySorting(p => p.Name, requestDto.SortByAscending);
                break;
            case nameof(ProductListRequestDto.FieldToSort.Name):
                query = query.ApplySorting(p => p.Name, requestDto.SortByAscending);
                break;
            case nameof(ProductListRequestDto.FieldToSort.DefaultAmountBeforeVatPerUnit):
                query = query.ApplySorting(p => p.DefaultAmountBeforeVatPerUnit, requestDto.SortByAscending);
                break;
            case nameof(ProductListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(p => p.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(ProductListRequestDto.FieldToSort.StockingQuantity):
                query = query.ApplySorting(
                    p => p.Stock != null ? p.Stock.StockingQuantity : 0,
                    requestDto.SortByAscending
                );
                break;
            default:
                throw new NotImplementedException();
        }

        (int page, int resultPerPage) = (requestDto.Page, requestDto.ResultsPerPage);
        Page<Product> queryResult = await _listFetchingService.GetPagedListAsync(query, page, resultPerPage);

        List<ProductBasicResponseDto> productResponseDtos = queryResult.Items
            .Select(p => new ProductBasicResponseDto(p, _authorizationService.GetProductExistingAuthorization(p)))
            .ToList();

        return new(productResponseDtos, queryResult.ItemCount, queryResult.PageCount);
    }

    public async Task<ProductDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.Products
            .AsSplitQuery()
            .Include(p => p.CreatedUser)
            .Include(p => p.LastUpdatedUser)
            .Include(p => p.DeletedUser)
            .Include(p => p.Categories)
            .Include(p => p.Stock)
            .Include(p => p.Photos)
            .Where(p => p.Id == id && p.DeletedDateTime == null)
            .Select(p => new ProductDetailResponseDto(p, _authorizationService.GetProductExistingAuthorization(p)))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    public async Task<int> CreateAsync(ProductCreateRequestDto requestDto)
    {
        if (!_authorizationService.CanCreateProduct())
        {
            throw new AuthorizationException();
        }

        _createValidator.ValidateAndThrow(requestDto);

        Dictionary<int, ProductCategory> productCategoryIdMap = new();
        if (requestDto.CategoryIds.Count > 0)
        {
            productCategoryIdMap = await _context.ProductCategories
                .Where(pc => requestDto.CategoryIds.Contains(pc.Id))
                .ToDictionaryAsync(pc => pc.Id, pc => pc);
        }

        Product product = new()
        {
            Name = requestDto.Name,
            Description = requestDto.Description,
            Unit = requestDto.Unit,
            DefaultAmountBeforeVatPerUnit = requestDto.DefaultAmountBeforeVatPerUnit,
            DefaultVatPercentagePerUnit = requestDto.DefaultVatPercentagePerUnit,
            IsForRetail = requestDto.IsForRetail,
            CreatedDateTime = _clock.Now,
            CreatedUserId = _callerDetailProvider.GetId(),
            Stock = new()
            {
                ResupplyThresholdQuantity = requestDto.ResupplyThresholdQuantity
            },
        };

        for (int index = 0; index < requestDto.CategoryIds.Count; index += 1)
        {
            int categoryId = requestDto.CategoryIds[index];
            productCategoryIdMap.TryGetValue(categoryId, out ProductCategory? category);
            if (category is null)
            {
                object[] propertyPathElements = new object[] { nameof(requestDto.CategoryIds), index };
                throw OperationException.NotFound(propertyPathElements, DisplayNames.Category);
            }

            product.Categories.Add(category);
        }

        // TODO: Implement photo handling.
        // foreach (PhotoCreateOrUpdateRequestDto photoRequestDto in requestDto.Photos)
        // {
        //     Photo photo = new()
        //     {
        //         Url = photoRequestDto.Url
        //     }
        // }

        _context.Products.Add(product);

        try
        {
            await _context.SaveChangesAsync();
            return product.Id;
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsForeignKeyConstraintViolation)
            {
                throw new ConcurrencyException();
            }

            if (handledResult.IsUniqueConstraintViolation)
            {
                object[] propertyPathElements = new object[] { nameof(requestDto.Name) };
                throw OperationException.Duplicated(propertyPathElements, DisplayNames.Name);
            }

            throw;
        }
    }

    public async Task UpdateAsync(int id, ProductUpdateRequestDto requestDto)
    {
        _updateValidator.ValidateAndThrow(requestDto);

        Product product = await _context.Products
            .Include(p => p.Categories)
            .Include(p => p.Stock)
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(p => p.Id == id && p.DeletedDateTime == null)
            ?? throw new NotFoundException();

        ProductExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetProductExistingAuthorization(product);
        if (!authorization.CanEdit)
        {
            throw new AuthorizationException();
        }

        product.Name = requestDto.Name;
        product.Description = requestDto.Description;
        product.Unit = requestDto.Unit;
        product.DefaultAmountBeforeVatPerUnit = requestDto.DefaultAmountBeforeVatPerUnit;
        product.DefaultVatPercentagePerUnit = requestDto.DefaultVatPercentagePerUnit;
        product.IsForRetail = requestDto.IsForRetail;
        product.IsDiscontinued = requestDto.IsDiscontinued;

        product.Stock ??= new();
        product.Stock.ResupplyThresholdQuantity = requestDto.ResupplyThresholdQuantity;

        product.LastUpdatedDateTime = _clock.Now;
        product.LastUpdatedUserId = _callerDetailProvider.GetId();
        
        List<ProductCategory> categoriesToBeDeleted = product.Categories
            .Where(pc => !requestDto.CategoryIds.Contains(pc.Id))
            .ToList();

        foreach (ProductCategory category in categoriesToBeDeleted)
        {
            product.Categories.Remove(category);
        }

        IEnumerable<int> beforeUpdatingCategoryIds = product.Categories.Select(p => p.Id);
        List<int> categoryIdsToBeAdded = requestDto.CategoryIds
            .Where(cid => !beforeUpdatingCategoryIds.Contains(cid))
            .ToList();

        Dictionary<int, ProductCategory> categoryIdToBeAddedMap = await _context.ProductCategories
            .Where(pc => categoryIdsToBeAdded.Contains(pc.Id))
            .ToDictionaryAsync(pc => pc.Id, pc => pc);
        
        for (int index = 0; index < categoryIdsToBeAdded.Count; index += 1)
        {
            int requestedCategoryId = categoryIdsToBeAdded[index];
            categoryIdToBeAddedMap.TryGetValue(requestedCategoryId, out ProductCategory? category);
            if (category is null)
            {
                int requestedIdIndex = requestDto.CategoryIds.IndexOf(requestedCategoryId);
                object[] propertyPathElements = new object[] { nameof(requestDto.CategoryIds), requestedIdIndex };
                throw OperationException.NotFound(propertyPathElements, DisplayNames.Category);
            }

            product.Categories.Add(category);
        }

        // TODO: Implement photo handling.

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict || handledResult.IsForeignKeyConstraintViolation)
            {
                throw new ConcurrencyException();
            }

            if (handledResult.IsUniqueConstraintViolation)
            {
                object[] propertyPathElements = new object[] { nameof(requestDto.Name) };
                throw OperationException.Duplicated(propertyPathElements, DisplayNames.Name);
            }

            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        Product product = await _context.Products
            .SingleOrDefaultAsync(p => p.Id == id && p.DeletedDateTime == null)
            ?? throw new NotFoundException();
            
        ProductExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetProductExistingAuthorization(product);
        if (!authorization.CanDelete)
        {
            throw new AuthorizationException();
        }

        product.DeletedDateTime = _clock.Now;
        product.DeletedUserId = _callerDetailProvider.GetId();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict || handledResult.IsForeignKeyConstraintViolation)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
    #endregion
}