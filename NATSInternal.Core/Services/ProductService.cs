namespace NATSInternal.Core.Services;

/// <inheritdoc cref="IProductService" />
internal class ProductService
    :
        UpsertableAbstractService<
            Product,
            ProductListRequestDto,
            ProductExistingAuthorizationResponseDto>,
        IProductService
{
    private readonly DatabaseContext _context;
    private readonly IMultiplePhotosService<Product, ProductPhoto> _photoService;

    public ProductService(
            DatabaseContext context,
            IMultiplePhotosService<Product, ProductPhoto> photoService,
            IAuthorizationInternalService authorizationService) : base(authorizationService)
    {
        _context = context;
        _photoService = photoService;
    }

    /// <inheritdoc />
    public async Task<ProductListResponseDto> GetListAsync(ProductListRequestDto requestDto)
    {
        IQueryable<Product> query = _context.Products;
        
        // Determine the field and the direction the sort.
        string sortingByField = requestDto.SortingByFieldName
                                ?? GetListSortingOptions().DefaultFieldName;
        bool sortingByAscending = requestDto.SortingByAscending
                                  ?? GetListSortingOptions().DefaultAscending;
        switch (sortingByField)
        {
            case nameof(OrderByFieldOption.CreatedDateTime):
                query = sortingByAscending
                    ? query
                        .OrderBy(p => p.CreatedDateTime)
                        .ThenBy(p => p.StockingQuantity)
                    : query
                        .OrderBy(p => p.CreatedDateTime)
                        .ThenByDescending(p => p.StockingQuantity);
                break;
            case nameof(OrderByFieldOption.StockingQuantity):
                query = sortingByAscending
                    ? query
                        .OrderBy(p => p.StockingQuantity)
                        .ThenBy(p => p.NormalizedName)
                    : query
                        .OrderBy(p => p.StockingQuantity)
                        .ThenByDescending(p => p.NormalizedName);
                break;
            default:
                throw new NotImplementedException();
        }

        // Filter by category name.
        if (requestDto.CategoryId != null)
        {
            query = query
                .Where(p => p.Category != null && p.Category.Id == requestDto.CategoryId);
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
                .ToNonDiacritics()
                .ToUpper();
            query = query.Where(p => p.NormalizedName.Contains(productNonDiacriticsName));
        }

        // Fetch the list of the entities.
        EntityListDto<Product> listDto = await GetListOfEntitiesAsync(query, requestDto);

        return new ProductListResponseDto
        {
            PageCount = listDto.PageCount,
            Items = listDto.Items?
                .Select(p => new ProductBasicResponseDto(p, GetExistingAuthorization(p)))
                .ToList()
                ?? new List<ProductBasicResponseDto>()
        };
    }

    /// <inheritdoc />
    public async Task<ProductDetailResponseDto> GetDetailAsync(
            int id,
            ProductDetailRequestDto requestDto)
    {
        Product product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.Id == id)
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException(
                nameof(Product),
                nameof(id),
                id.ToString());

        ProductExistingAuthorizationResponseDto authorizationResponseDto;
        authorizationResponseDto = GetExistingAuthorization(product);

        return new ProductDetailResponseDto(product, authorizationResponseDto);
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(ProductUpsertRequestDto requestDto)
    {
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
            CreatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            LastUpdatedDateTime = null,
            StockingQuantity = 0,
            BrandId = requestDto.BrandId,
            CategoryId = requestDto.CategoryId,
            Photos = new List<ProductPhoto>()
        };
        _context.Products.Add(product);

        // Create thumbnail if specified.
        if (requestDto.ThumbnailFile != null)
        {
            product.ThumbnailUrl = await _photoService
                .CreateAsync(requestDto.ThumbnailFile, true);
        }

        // Create photos if specified.
        if (requestDto.Photos != null)
        {
            await _photoService.CreateMultipleAsync(product, requestDto.Photos);
        }

        try
        {
            await _context.SaveChangesAsync();
            return product.Id;
        }
        catch (DbUpdateException exception)
        {
            // Delete the recently created thumbnail if exists.
            if (product.ThumbnailUrl != null)
            {
                _photoService.Delete(product.ThumbnailUrl);
            }

            // Delete the recently created photos if exists.
            if (product.Photos?.Count > 0)
            {
                foreach (ProductPhoto photo in product.Photos)
                {
                    _photoService.Delete(photo.Url);
                }
            }

            
            // Handle the concurrency-related operation.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle the business-logic-related exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                HandleDeleteOrUpdateException(sqlException);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, ProductUpsertRequestDto requestDto)
    {
        // Fetch the entity with given id from the database and ensure that it exists.
        Product product = await _context.Products.FindAsync(id)
            ?? throw new NotFoundException(
                nameof(Product),
                nameof(id),
                id.ToString());

        // Update the entity's properties.
        product.Name = requestDto.Name;
        product.Description = requestDto.Description;
        product.Unit = requestDto.Unit;
        product.DefaultAmountBeforeVatPerUnit = requestDto.DefaultAmountBeforeVatPerUnit;
        product.DefaultVatPercentage = requestDto.DefaultVatPercentage;
        product.IsForRetail = requestDto.IsForRetail;
        product.IsDiscontinued = requestDto.IsDiscontinued;
        product.CategoryId = requestDto.CategoryId;
        product.BrandId = requestDto.BrandId;
        product.LastUpdatedDateTime = DateTime.UtcNow.ToApplicationTime();

        // Prepare lists of urls to be deleted later when the operation
        // is succeeded or failed.
        List<string> urlsToBeDeletedWhenFailed = new List<string>();
        List<string> urlsToBeDeletedWhenSucceeded = new List<string>();

        // Update the thumbnail if changed.
        if (requestDto.ThumbnailChanged)
        {
            // Delete the current thumbnail if exists.
            if (product.ThumbnailUrl != null)
            {
                urlsToBeDeletedWhenSucceeded.Add(product.ThumbnailUrl);
            }

            // Create a new one if the request contains data for it.
            if (requestDto.ThumbnailFile != null)
            {
                string thumbnailUrl = await _photoService
                    .CreateAsync(requestDto.ThumbnailFile, true);
                product.ThumbnailUrl = thumbnailUrl;
                urlsToBeDeletedWhenFailed.Add(product.ThumbnailUrl);
            }
        }

        // Update the photos if changed.
        if (requestDto.Photos?.Count > 0)
        {
            (List<string>, List<string>) photoUpdateResult = await _photoService
                .UpdateMultipleAsync(product, requestDto.Photos);
            urlsToBeDeletedWhenSucceeded.AddRange(photoUpdateResult.Item1);
            urlsToBeDeletedWhenFailed.AddRange(photoUpdateResult.Item2);
        }

        // Save changes or throw exeption if any error occurs.
        try
        {
            await _context.SaveChangesAsync();

            // The product can be updated successfully.
            // Delete the specified thumbnail and associated photos.
            foreach (string url in urlsToBeDeletedWhenSucceeded)
            {
                _photoService.Delete(url);
            }
        }
        catch (DbUpdateException exception)
        {
            // Delete the recently added thumbnail and photos.
            foreach (string url in urlsToBeDeletedWhenFailed)
            {
                _photoService.Delete(url);
            }
            
            // Handle the concurrency-related operation.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle the business-logic-related exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                HandleDeleteOrUpdateException(sqlException);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity from the database and ensure the entity exists.
        Product product = await _context.Products
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(p => p.Id == id && !p.IsDeleted)
            ?? throw new NotFoundException();

        // Remove the product and all associated photos.
        _context.Products.Remove(product);

        foreach (ProductPhoto photo in product.Photos)
        {
            _context.ProductPhotos.Remove(photo);
        }

        // Performing deleting operation.
        try
        {
            await _context.SaveChangesAsync();

            // The product can be deleted successfully.
            // Delete the thumbnail.
            if (product.ThumbnailUrl != null)
            {
                _photoService.Delete(product.ThumbnailUrl);
            }

            // Delete the photos.
            foreach (ProductPhoto photo in product.Photos)
            {
                _photoService.Delete(photo.Url);
            }
        }
        catch (DbUpdateException exception)
        {
            // Handle the concurrency-related exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }
            
            // Handle the business-logic-related exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
                // Entity is referenced by some other column's entity, perform soft delete
                // instead.
                if (exceptionHandler.IsDeleteOrUpdateRestricted)
                {
                    product.IsDeleted = true;
                    await _context.SaveChangesAsync();
                    return;
                }
            }

            throw;
        }
    }

    /// <inheritdoc cref="IProductService.GetListSortingOptions" />
    public override ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions;
        fieldOptions = new List<ListSortingByFieldResponseDto>
        {
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.CreatedDateTime),
                DisplayName = DisplayNames.Amount
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.StockingQuantity),
                DisplayName = DisplayNames.StatsDateTime
            }
        };

        return new ListSortingOptionsResponseDto
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions
                .Single(i => i.Name == nameof(OrderByFieldOption.CreatedDateTime))
                .Name,
            DefaultAscending = false
        };
    }

    /// <summary>
    /// Handle delete or update operation exception from the database and
    /// convert it into appropriate OperationException.
    /// </summary>
    /// <param name="exception">
    /// The inner exception of the DbUpdateExeception, thrown by the database
    /// after committing changes.
    /// </param>
    /// <exception cref="OperationException"></exception>
    private void HandleDeleteOrUpdateException(MySqlException exception)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(exception);
        // Handle foreign key exception.
        if (exceptionHandler.IsForeignKeyNotFound)
        {
            string errorMessage;
            if (exceptionHandler.ViolatedFieldName == "brand_id")
            {
                errorMessage = ErrorMessages.NotFound
                    .ReplaceResourceName(DisplayNames.Get(nameof(Brand)));
                throw new OperationException(errorMessage);
            }

            errorMessage = ErrorMessages.NotFound
                .ReplaceResourceName(DisplayNames.Get(nameof(ProductCategory)));

            throw new OperationException(errorMessage);
        }

        // Handle unique conflict exception.
        if (exceptionHandler.IsUniqueConstraintViolated)
        {
            string errorMessage = ErrorMessages.Duplicated
                .ReplacePropertyName(DisplayNames.Get(nameof(Product.Name)));
            throw new OperationException(nameof(Product.Name), errorMessage);
        }
    }
}