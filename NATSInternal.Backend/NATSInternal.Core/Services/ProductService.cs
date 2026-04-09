using Bogus.Extensions;

namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class ProductService : IProductService
{
    #region Fields
    private readonly DatabaseContext _context;
    private readonly IListQueryService _listQueryService;
    private readonly IPhotoService<Product> _photoService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<ProductListRequestDto> _listValidator;
    private readonly IValidator<ProductUpsertRequestDto> _upsertValidator;
    #endregion

    #region Constructors
    public ProductService(
            DatabaseContext context,
            IListQueryService listQueryService,
            IPhotoService<Product> photoService,
            IAuthorizationInternalService authorizationService,
            IDbExceptionHandler exceptionHandler,
            IValidator<ProductListRequestDto> listValidator,
            IValidator<ProductUpsertRequestDto> upsertValidator)
    {
        _context = context;
        _listQueryService = listQueryService;
        _photoService = photoService;
        _authorizationService = authorizationService;
        _exceptionHandler = exceptionHandler;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<ProductListResponseDto> GetListAsync(
            ProductListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<Product> query = _context.Products;
        
        string sortingByField = requestDto.SortByFieldName ?? GetListSortingOptions().DefaultFieldName;
        bool sortByAscending = requestDto.SortByAscending ?? GetListSortingOptions().DefaultAscending;
        switch (sortingByField)
        {
            case nameof(ProductListRequestDto.FieldToSort.CreatedDateTime) or null:
                query = query
                    .ApplySorting(p => p.CreatedDateTime, sortByAscending)
                    .ThenApplySorting(p => p.StockingQuantity, sortByAscending);

                break;
            case nameof(ProductListRequestDto.FieldToSort.StockingQuantity):
                query = query
                    .ApplySorting(p => p.StockingQuantity, sortByAscending)
                    .ThenApplySorting(p => p.NormalizedName, sortByAscending);

                break;
            default:
                throw new NotImplementedException();
        }

        // Filter by category name.
        if (requestDto.CategoryId != null)
        {
            query = query.Where(p => p.Category != null && p.Category.Id == requestDto.CategoryId);
        }

        // Filter by brand id.
        if (requestDto.BrandId != null)
        {
            query = query.Where(p => p.BrandId == requestDto.BrandId);
        }

        // Filter by product name.
        if (requestDto.ProductName != null)
        {
            string productNonDiacriticsName = requestDto.ProductName
                .RemoveDiacritics()
                .ToUpper();
            query = query.Where(p => p.NormalizedName.Contains(productNonDiacriticsName));
        }

        return await _listQueryService.GetPagedListAsync(
            query,
            requestDto,
            (products, pageCount) => new ProductListResponseDto(products, pageCount),
            cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<ProductDetailResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Product product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        ProductExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetExistingAuthorization<Product, ProductExistingAuthorizationResponseDto>();

        return new ProductDetailResponseDto(product, authorizationResponseDto);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(
            ProductUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _upsertValidator.Validate(
            requestDto,
            options => options.ThrowOnFailures().IncludeRuleSets("Create").IncludeRulesNotInRuleSet());

        // Initialize product entity.
        Product product = new Product
        {
            Name = requestDto.Name,
            Description = requestDto.Description,
            Unit = requestDto.Unit,
            DefaultAmountBeforeVatPerUnit = requestDto.DefaultAmountBeforeVatPerUnit,
            DefaultVatPercentage = requestDto.DefaultVatPercentage,
            IsForRetail = requestDto.IsForRetail,
            IsDiscontinued = requestDto.IsDiscontinued,
            LastUpdatedDateTime = null,
            StockingQuantity = 0,
            BrandId = requestDto.BrandId
        };
        _context.Products.Add(product);

        // Fetch product category or initialize if not exists.
        if (requestDto.Category is not null)
        {
            product.Category = await _context.ProductCategories
                .SingleOrDefaultAsync(pc => pc.Name == requestDto.Category.Name, cancellationToken)
                ?? new() { Name = requestDto.Category.Name };
        }

        // Create photos.
        await _photoService.CreateMultipleAsync(product, requestDto.Photos, cancellationToken);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
        catch (Exception exception)
        {
            foreach (Photo photo in product.Photos)
            {
                _photoService.Delete(photo.Url);
            }

            if (exception is not DbUpdateException dbUpdateException)
            {
                throw;
            }

            CoreException? convertedException = TryConvertDbUpdateException(dbUpdateException);
            if (convertedException is null)
            {
                throw;
            }

            throw convertedException;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(
            Guid id,
            ProductUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _upsertValidator.Validate(
            requestDto,
            options => options.ThrowOnFailures().IncludeRuleSets("CreateAndUpdate").IncludeRulesNotInRuleSet());

        // Fetch the entity with given id from the database and ensure that it exists.
        Product product = await _context.Products
            .Include(p => p.Category)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        // Update the entity's properties.
        product.Name = requestDto.Name;
        product.Description = requestDto.Description;
        product.Unit = requestDto.Unit;
        product.DefaultAmountBeforeVatPerUnit = requestDto.DefaultAmountBeforeVatPerUnit;
        product.DefaultVatPercentage = requestDto.DefaultVatPercentage;
        product.IsForRetail = requestDto.IsForRetail;
        product.IsDiscontinued = requestDto.IsDiscontinued;
        product.BrandId = requestDto.BrandId;
        product.LastUpdatedDateTime = DateTime.UtcNow.ToApplicationTime();

        if (requestDto.Category == null)
        {
            product.Category = null;
        }
        else if (product.Category?.Name != requestDto.Category.Name)
        {
            ProductCategory? oldCategory = product.Category;
            if (oldCategory is not null)
            {
                _context.ProductCategories.Remove(oldCategory);
            }

            product.Category = await _context.ProductCategories
                .SingleOrDefaultAsync(pc => pc.Name == requestDto.Category.Name, cancellationToken)
                ?? new() { Name = requestDto.Category.Name };
        }

        // Prepare lists of urls to be deleted later when the operation is succeeded or failed.
        List<string> urlsToBeDeletedWhenFailed = new();
        List<string> urlsToBeDeletedWhenSucceeded = new();
        (urlsToBeDeletedWhenSucceeded, urlsToBeDeletedWhenFailed) = await _photoService.UpdateMultipleAsync(
            product,
            requestDto.Photos,
            cancellationToken);

        // Save changes or throw exeption if any error occurs.
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            // The product can be updated successfully, delete the specified thumbnail and associated photos.
            foreach (string url in urlsToBeDeletedWhenSucceeded)
            {
                _photoService.Delete(url);
            }
        }
        catch (Exception exception)
        {
            // Delete the recently added thumbnail and photos.
            foreach (string url in urlsToBeDeletedWhenFailed)
            {
                _photoService.Delete(url);
            }

            if (exception is not DbUpdateException dbUpdateException)
            {
                throw;
            }

            CoreException? convertedException = TryConvertDbUpdateException(dbUpdateException);
            if (convertedException is null)
            {
                throw;
            }

            throw convertedException;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Fetch the entity from the database and ensure the entity exists.
        Product product = await _context.Products
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException();

        // Remove the product and all associated photos.
        _context.Products.Remove(product);

        // Performing deleting operation.
        try
        {
            await _context.SaveChangesAsync();

            // The product can be deleted successfully, delete all the photos in the storage.
            foreach (string url in product.Photos.Select(p => p.Url))
            {
                _photoService.Delete(url);
            }
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
                throw OperationException.DeleteRestricted(DisplayNames.Product);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public bool GetCreatingPermission()
    {
        return _authorizationService.CanCreate<Product>();
    }

    /// <inheritdoc />
    public ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions = new()
        {
            new()
            {
                Name = nameof(ProductListRequestDto.FieldToSort.CreatedDateTime),
                DisplayName = DisplayNames.Amount
            },
            new()
            {
                Name = nameof(ProductListRequestDto.FieldToSort.StockingQuantity),
                DisplayName = DisplayNames.StatsDateTime
            }
        };

        return new()
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions
                .Where(i => i.Name == nameof(ProductListRequestDto.FieldToSort.CreatedDateTime))
                .Select(i => i.Name)
                .Single(),
            DefaultAscending = false
        };
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Convert the exception which is thrown by the database during the creating or updating operation into an instance
    /// of <see cref="CoreException"/> .
    /// </summary>
    /// <param name="exception">
    /// An instance of the <see cref="DbUpdateException"/> class, contanining the details of the error.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CoreException"/> class, representing the converted exception (when successful) or
    /// <see langword="null"/> (if failure).
    /// </returns>
    private CoreException? TryConvertDbUpdateException(DbUpdateException exception)
    {
        DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
        if (handledResult is null)
        {
            return null;
        }

        if (handledResult.IsConcurrencyConflict)
        {
            return new ConcurrencyException();
        }

        if (handledResult.IsForeignKeyConstraintViolation &&
            handledResult.ViolatedPropertyName == nameof(ProductUpsertRequestDto.BrandId))
        {
            return OperationException.NotFound(
                new object[] { nameof(ProductUpsertRequestDto.BrandId) },
                DisplayNames.Brand
            );
        }

        if (handledResult.IsUniqueConstraintViolation)
        {
            return OperationException.Duplicated(
                new object[] { nameof(ProductUpsertRequestDto.Name) },
                DisplayNames.Name
            );
        }

        return null;
    }
    #endregion
}