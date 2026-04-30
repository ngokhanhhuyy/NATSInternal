using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;
using ApplicationException = NATSInternal.Core.Common.Exceptions.ApplicationException;

namespace NATSInternal.Core.Features.Products;

internal class ProductCategoryService : IProductCategoryService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IValidator<ProductCategoryUpsertRequestDto> _upsertValidator;
    private readonly IDbExceptionHandler _exceptionHandler;
    #endregion

    #region Constructors
    public ProductCategoryService(
        AppDbContext context,
        IAuthorizationInternalService authorizationService,
        IValidator<ProductCategoryUpsertRequestDto> upsertValidator,
        IDbExceptionHandler exceptionHandler)
    {
        _context = context;
        _authorizationService = authorizationService;
        _upsertValidator = upsertValidator;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<List<ProductCategoryBasicResponseDto>> GetAllAsync()
    {
        return await _context.ProductCategories
            .OrderBy(pc => pc.Name)
            .Select(pc => new ProductCategoryBasicResponseDto(
                pc,
                _authorizationService.GetProductCategoryExistingAuthorization(pc)))
            .ToListAsync();
    }

    public async Task<ProductCategoryDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.ProductCategories
            .Where(pc => pc.Id == id)
            .Select(pc => new ProductCategoryDetailResponseDto(
                pc,
                _authorizationService.GetProductCategoryExistingAuthorization(pc)))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    public async Task<int> CreateAsync(ProductCategoryUpsertRequestDto requestDto)
    {
        if (!_authorizationService.CanCreateProductCategory())
        {
            throw new AuthorizationException();
        }

        _upsertValidator.ValidateAndThrow(requestDto);

        ProductCategory category = new()
        {
            Name = requestDto.Name
        };

        _context.ProductCategories.Add(category);
        
        ApplicationException? exception = await SaveChangesAsync();
        if (exception is not null)
        {
            throw exception;
        }

        return category.Id;
    }

    public async Task UpdateAsync(int id, ProductCategoryUpsertRequestDto requestDto)
    {
        _upsertValidator.ValidateAndThrow(requestDto);

        ProductCategory category = await _context.ProductCategories
            .SingleOrDefaultAsync(pc => pc.Id == id)
            ?? throw new NotFoundException();

        ProductCategoryExistingAuthorizationResponseDto authorization = _authorizationService
            .GetProductCategoryExistingAuthorization(category);
        if (!authorization.CanEdit)
        {
            throw new AuthorizationException();
        }

        category.Name = requestDto.Name;

        ApplicationException? exception = await SaveChangesAsync();
        if (exception is not null)
        {
            throw exception;
        }
    }

    public async Task DeleteAsync(int id)
    {
        ProductCategory category = await _context.ProductCategories
            .SingleOrDefaultAsync(pc => pc.Id == id)
            ?? throw new NotFoundException();

        ProductCategoryExistingAuthorizationResponseDto authorization = _authorizationService
            .GetProductCategoryExistingAuthorization(category);
        if (!authorization.CanDelete)
        {
            throw new AuthorizationException();
        }

        _context.ProductCategories.Remove(category);

        ApplicationException? exception = await SaveChangesAsync();
        if (exception is not null)
        {
            throw exception;
        }
    }
    #endregion

    #region PrivateMethods
    private async Task<ApplicationException?> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return null;
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            if (handledResult.IsUniqueConstraintViolation)
            {
                object[] propertyPathElements = new string[] { nameof(ProductCategoryUpsertRequestDto.Name) };
                return OperationException.Duplicated(propertyPathElements, DisplayNames.Name);
            }

            throw;
        }
    }
    #endregion
}
