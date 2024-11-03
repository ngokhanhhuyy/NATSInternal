namespace NATSInternal.Services;

/// <inheritdoc cref="IProductCategoryService" />
internal class ProductCategoryService
    :
        UpsertableAbstractService<
            ProductCategory,
            ProductCategoryListRequestDto,
            ProductCategoryExistingAuthorizationResponseDto>,
        IProductCategoryService
{
    private readonly DatabaseContext _context;

    public ProductCategoryService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService) : base(authorizationService)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<ProductCategoryListResponseDto> GetListAsync(
            ProductCategoryListRequestDto requestDto)
    {
        // Initialize query.
        IQueryable<ProductCategory> query = _context.ProductCategories
                .OrderBy(productCategory => productCategory.Id);

        EntityListDto<ProductCategory> listDto;
        listDto = await GetListOfEntitiesAsync(query, requestDto);

        return new ProductCategoryListResponseDto
        {
            PageCount = listDto.PageCount,
            Items = listDto.Items
                .Select(productCategory =>
                {
                    ProductCategoryExistingAuthorizationResponseDto authorization;
                    authorization = GetExistingAuthorization(productCategory);

                    return new ProductCategoryResponseDto(productCategory, authorization);
                }).ToList()
        };
    }

    /// <inheritdoc />
    public async Task<List<ProductCategoryMinimalResponseDto>> GetAllAsync()
    {
        return await _context.ProductCategories
            .OrderBy(productCategory => productCategory.Id)
            .Select(productCategory => new ProductCategoryMinimalResponseDto(productCategory))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<ProductCategoryResponseDto> GetDetailAsync(int id)
    {
        return await _context.ProductCategories
            .Where(pc => pc.Id == id)
            .Select(pc => new ProductCategoryResponseDto(pc, GetExistingAuthorization(pc)))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(ProductCategory),
                nameof(id),
                id.ToString());
    }

    /// <inheritdoc />
    public async Task<int> CreateAsyns(ProductCategoryRequestDto requestDto)
    {
        // Initialize the entity.
        ProductCategory productCategory = new ProductCategory
        {
            Name = requestDto.Name,
            CreatedDateTime = DateTime.UtcNow.ToApplicationTime()
        };

        // Perform the creating operation.
        try
        {
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();
            return productCategory.Id;
        }
        // Handle the exception.
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
                HandleException(sqlException);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, ProductCategoryRequestDto requestDto)
    {
        // Fetch the entity with the specified id from the database.
        ProductCategory productCategory = await _context.ProductCategories
            .SingleOrDefaultAsync(pc => pc.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(ProductCategory),
                nameof(id),
                id.ToString());
        
        // Update the property value.
        productCategory.Name = requestDto.Name;
        
        // Perform the updating operation.
        try
        {
            await _context.SaveChangesAsync();
        }
        // Handle the exception.
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
                HandleException(sqlException);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity with the specified id from the database.
        ProductCategory productCategory = await _context.ProductCategories
            .SingleOrDefaultAsync(pc => pc.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(ProductCategory),
                nameof(id),
                id.ToString());
        
        // Perform the deleting operation.
        try
        {
            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();
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
                HandleException(sqlException);
            }

            throw;
        }
    }
    
    /// <inheritdoc cref="IProductCategoryService.GetListSortingOptions" />
    public override ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions;
        fieldOptions = new List<ListSortingByFieldResponseDto>
        {
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.CreatedDateTime),
                DisplayName = DisplayNames.CreatedDateTime
            }
        };

        return new ListSortingOptionsResponseDto
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions.Single().Name,
            DefaultAscending = true
        };
    }

    /// <summary>
    /// Handle the exception which is thrown by the database during the creating or updating
    /// operation.
    /// </summary>
    /// <remarks>
    /// This method only convert the specified <c>exception</c> into the mapped
    /// <see cref="OperationException"/> under the defined circumstance.
    /// </remarks>
    /// <param name="exception">
    /// An instance of the <see cref="MySqlException"/> class, contanining the details of the
    /// error.
    /// </param>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the <c>exception</c> indicates that the error occurs due to the unique
    /// constraint violation during the operation.<br/>
    /// - When the <c>exception</c> indicates that the error occurs due to the restriction
    /// caused by the existence of some related resource(s).
    /// </exception>
    private static void HandleException(MySqlException exception)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(exception);
        string errorMessage;
        
        if (exceptionHandler.IsUniqueConstraintViolated)
        {
            errorMessage = ErrorMessages.UniqueDuplicated
                .ReplacePropertyName(DisplayNames.Get(nameof(ProductCategory.Name)));
            throw new OperationException(errorMessage);
        }
        
        if (exceptionHandler.IsDeleteOrUpdateRestricted)
        {
            errorMessage = ErrorMessages.DeleteRestricted
                .ReplaceResourceName(DisplayNames.Category);
            throw new OperationException(errorMessage);
        }
    }
}
