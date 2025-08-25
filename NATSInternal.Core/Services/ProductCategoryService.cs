namespace NATSInternal.Core.Services;

/// <inheritdoc cref="IProductCategoryService" />
internal class ProductCategoryService : IProductCategoryService
{
    #region Fields
    private readonly DatabaseContext _context;
    private readonly IListQueryService _listQueryService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<ProductCategoryListRequestDto> _listValidator;
    private readonly IValidator<ProductCategoryUpsertRequestDto> _upsertValidator;
    #endregion

    public ProductCategoryService(
            DatabaseContext context,
            IListQueryService listQueryService,
            IAuthorizationInternalService authorizationService,
            IDbExceptionHandler exceptionHandler,
            IValidator<ProductCategoryListRequestDto> listValidator,
            IValidator<ProductCategoryUpsertRequestDto> upsertValidator)
    {
        _context = context;
        _listQueryService = listQueryService;
        _authorizationService = authorizationService;
        _exceptionHandler = exceptionHandler;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
    }

    /// <inheritdoc />
    public async Task<ProductCategoryListResponseDto> GetListAsync(
            ProductCategoryListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        _listValidator.ValidateAndThrow(requestDto);

        // Initialize query.
        IQueryable<ProductCategory> query = _context.ProductCategories.OrderBy(productCategory => productCategory.Id);

        return await _listQueryService.GetPagedListAsync(
            query,
            requestDto,
            (entities, pageCount) => new ProductCategoryListResponseDto(entities, pageCount),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<ProductCategoryMinimalResponseDto>> GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .OrderBy(productCategory => productCategory.Id)
            .Select(productCategory => new ProductCategoryMinimalResponseDto(productCategory))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ProductCategoryResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        ProductCategory category = await _context.ProductCategories
            .SingleOrDefaultAsync(pc => pc.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        ProductCategoryExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetExistingAuthorization<ProductCategory, ProductCategoryExistingAuthorizationResponseDto>();

        return new(category, authorizationResponseDto);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(
            ProductCategoryUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        _upsertValidator.ValidateAndThrow(requestDto);

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
            await _context.SaveChangesAsync(cancellationToken);
            
            return productCategory.Id;
        }
        // Handle the exception.
        catch (DbUpdateException exception)
        {
            CoreException? convertedException = TryConvertDbUpdateException(exception);
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
            ProductCategoryUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate data from the request.
        requestDto.TransformValues();
        _upsertValidator.ValidateAndThrow(requestDto);

        // Fetch the entity with the specified id from the database.
        ProductCategory productCategory = await _context.ProductCategories
            .SingleOrDefaultAsync(pc => pc.Id == id, cancellationToken)
            ?? throw new NotFoundException();
        
        // Update the property value.
        productCategory.Name = requestDto.Name;
        
        // Perform the updating operation.
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            CoreException? convertedException = TryConvertDbUpdateException(exception);
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
        try
        {
            await _context.ProductCategories
                .Where(pc => pc.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            // Handle the concurrency-related exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
    
    /// <inheritdoc />
    public ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions = new()
        {
            new()
            {
                Name = nameof(ProductCategoryListRequestDto.FieldToSort.CreatedDateTime),
                DisplayName = DisplayNames.CreatedDateTime
            }
        };

        return new()
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions.Single().Name,
            DefaultAscending = true
        };
    }

    /// <inheritdoc />
    public bool GetCreatingPermission()
    {
        return _authorizationService.CanCreate<ProductCategory>();
    }

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

        if (handledResult.IsUniqueConstraintViolation)
        {
            return OperationException.Duplicated(
                new object[] { nameof(ProductCategoryUpsertRequestDto.Name) },
                DisplayNames.Name
            );
        }

        return null;
    }
}
