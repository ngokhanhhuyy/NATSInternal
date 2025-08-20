namespace NATSInternal.Core.Services;

/// <inheritdoc cref="IBrandService" />
internal class BrandService
    :
        UpsertableAbstractService<
            Brand,
            BrandListRequestDto,
            BrandExistingAuthorizationResponseDto>,
        IBrandService
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService<Brand> _photoService;

    public BrandService(
            DatabaseContext context,
            IPhotoService<Brand> photoService,
            IAuthorizationInternalService authorizationService) : base(authorizationService)
    {
        _context = context;
        _photoService = photoService;
    }

    /// <inheritdoc />
    public async Task<BrandListResponseDto> GetListAsync(BrandListRequestDto requestDto)
    {
        IQueryable<Brand> query = _context.Brands.OrderBy(b => b.Id);

        EntityListDto<Brand> entityListDto = await GetListOfEntitiesAsync(query, requestDto);

        return new BrandListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items?
                .Select(brand =>
                {
                    BrandExistingAuthorizationResponseDto authorization;
                    authorization = GetExistingAuthorization(brand);

                    return new BrandBasicResponseDto(brand, authorization);
                }).ToList()
                ?? new List<BrandBasicResponseDto>()
        };
    }

    /// <inheritdoc />
    public async Task<List<BrandMinimalResponseDto>> GetAllAsync()
    {
        return await _context.Brands
            .OrderBy(brand => brand.Id)
            .Select(brand => new BrandMinimalResponseDto(brand))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<BrandDetailResponseDto> GetDetailAsync(int id)
    {
        Brand brand = await _context.Brands
            .Include(b => b.Country)
            .Where(b => b.Id == id)
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException(nameof(Brand), nameof(id), id.ToString());

        return new BrandDetailResponseDto(brand, GetExistingAuthorization(brand));
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(BrandUpsertRequestDto requestDto)
    {
        string thumbnailUrl = null;
        if (requestDto.ThumbnailFile != null)
        {
            thumbnailUrl = await _photoService.CreateAsync(requestDto.ThumbnailFile, true);
        }

        Brand brand = new Brand
        {
            Name = requestDto.Name,
            Website = requestDto.Website,
            SocialMediaUrl = requestDto.SocialMediaUrl,
            PhoneNumber = requestDto.PhoneNumber,
            Email = requestDto.Email,
            Address = requestDto.Address,
            ThumbnailUrl = thumbnailUrl,
            CountryId = requestDto.CountryId
        };
        
        try
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return brand.Id;
        }
        catch (DbUpdateException exception)
        {
            _photoService.Delete(brand.ThumbnailUrl);
            if (exception.InnerException is MySqlException sqlException)
            {
                HandleDbUpdateException(sqlException);
            }
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, BrandUpsertRequestDto requestDto)
    {
        // Fetch the b from the database and ensure it exists.
        Brand brand = await _context.Brands.SingleOrDefaultAsync(b => b.Id == id)
            ?? throw new NotFoundException(
                nameof(Brand),
                nameof(id),
                id.ToString());

        // Update thumbnail if changed.
        string urlToBeDeletedWhenFailed = null;
        string urlToBeDeletedWhenSucceeded = null;
        if (requestDto.ThumbnailChanged)
        {
            // Delete the current thumbnail if exists.
            if (brand.ThumbnailUrl != null)
            {
                urlToBeDeletedWhenSucceeded = brand.ThumbnailUrl;
            }

            if (requestDto.ThumbnailFile != null)
            {
                brand.ThumbnailUrl = await _photoService.CreateAsync(
                    requestDto.ThumbnailFile,
                    true);
                urlToBeDeletedWhenFailed = brand.ThumbnailUrl;
            }
        }

        // Update the other properties.
        brand.Name = requestDto.Name;
        brand.Website = requestDto.Website;
        brand.SocialMediaUrl = requestDto.SocialMediaUrl;
        brand.PhoneNumber = requestDto.PhoneNumber;
        brand.Email = requestDto.Email;
        brand.Address = requestDto.Address;

        try
        {
            await _context.SaveChangesAsync();
            
            // The b has been updated successfully, delete the old photos.
            if (urlToBeDeletedWhenSucceeded != null)
            {
                _photoService.Delete(urlToBeDeletedWhenSucceeded);
            }
        }
        catch (DbUpdateException exception)
        {
            if (urlToBeDeletedWhenFailed != null)
            {
                _photoService.Delete(urlToBeDeletedWhenFailed);
            }
            
            if (exception.InnerException is MySqlException sqlException)
            {
                HandleDbUpdateException(sqlException);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        Brand brand = await _context.Brands.FindAsync(id)
            ?? throw new NotFoundException(
                nameof(Brand),
                nameof(id),
                id.ToString());

        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync();

        if (brand.ThumbnailUrl != null)
        {
            _photoService.Delete(brand.ThumbnailUrl);
        }
    }

    /// <inheritdoc cref="IBrandService.GetListSortingOptions" />
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
    /// Handle the <c>MySqlException</c> that is thrown by the database during the updating
    /// operation and convert into the corresponding meaningful expcetion.
    /// </summary>
    /// <param name="exception">
    /// The exception thrown by the database during the updating operation.
    /// </param>
    /// <exception cref="OperationException">
    /// Thrown when the <c>Country</c> with the specified ID doesn't exist or when
    /// the specified name is duplicated.
    /// </exception>
    private static void HandleDbUpdateException(MySqlException exception)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(exception);
        string errorMessage;
        
        if (exceptionHandler.IsForeignKeyNotFound)
        {
            errorMessage = ErrorMessages.NotFound
                .ReplaceResourceName(DisplayNames.Country);
            throw new OperationException("country.id", errorMessage);
        }
        
        if (exceptionHandler.IsUniqueConstraintViolated)
        {
            errorMessage = ErrorMessages.Duplicated
                .ReplaceResourceName(DisplayNames.Name);
            throw new OperationException("name", errorMessage);
        }
    }
}
