﻿namespace NATSInternal.Services;

/// <inheritdoc />
internal class BrandService
    :
        UpsertableAbstractService<Brand, BrandListRequestDto>,
        IBrandService
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService<Brand> _photoService;
    private readonly IAuthorizationInternalService _authorizationService;

    public BrandService(
            DatabaseContext context,
            IPhotoService<Brand> photoService,
            IAuthorizationInternalService authorizationService)
    {
        _context = context;
        _photoService = photoService;
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public async Task<BrandListResponseDto> GetListAsync(BrandListRequestDto requestDto)
    {
        IQueryable<Brand> query = _context.Brands.OrderBy(b => b.Id);

        EntityListDto<Brand> entityListDto = await GetListOfEntitiesAsync(query, requestDto);

        return new BrandListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items
                .Select(b => new BrandBasicResponseDto(
                    b,
                    _authorizationService.GetBrandAuthorization()))
                .ToList(),
            Authorization = _authorizationService.GetBrandListAuthorization()
        };
    }

    /// <inheritdoc />
    public async Task<List<BrandBasicResponseDto>> GetAllAsync()
    {
        return await _context.Brands
            .OrderBy(brand => brand.Id)
            .Select(brand => new BrandBasicResponseDto(
                brand,
                _authorizationService.GetBrandAuthorization()))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<BrandDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.Brands
            .Include(brand => brand.Country)
            .Where(brand => brand.Id == id)
            .Select(brand => new BrandDetailResponseDto(
                brand,
                _authorizationService.GetBrandAuthorization()))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(Brand),
                nameof(id),
                id.ToString());
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(BrandRequestDto requestDto)
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
    public async Task UpdateAsync(int id, BrandRequestDto requestDto)
    {
        // Fetch the brand from the database and ensure it exists.
        Brand brand = await _context.Brands.SingleOrDefaultAsync(b => b.Id == id)
            ?? throw new ResourceNotFoundException(
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
        brand.CountryId = requestDto.CountryId;

        try
        {
            await _context.SaveChangesAsync();
            
            // The brand has been updated successfully, delete the old photos.
            _photoService.Delete(urlToBeDeletedWhenSucceeded);
        }
        catch (DbUpdateException exception)
        {
            _photoService.Delete(urlToBeDeletedWhenFailed);
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
            ?? throw new ResourceNotFoundException(
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
            errorMessage = ErrorMessages.UniqueDuplicated
                .ReplaceResourceName(DisplayNames.Name);
            throw new OperationException("name", errorMessage);
        }
    }
}
