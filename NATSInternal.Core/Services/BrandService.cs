namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class BrandService : IBrandService
{
    #region Fields
    private readonly DatabaseContext _context;
    private readonly IPhotoService<Brand> _photoService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IListQueryService _listQueryService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<BrandListRequestDto> _listValidator;
    private readonly IValidator<BrandUpsertRequestDto> _upsertValidator;
    #endregion

    #region Constructors
    public BrandService(
            DatabaseContext context,
            IPhotoService<Brand> photoService,
            IAuthorizationInternalService authorizationService,
            IListQueryService listQueryService,
            IDbExceptionHandler exceptionHandler,
            IValidator<BrandListRequestDto> listValidator,
            IValidator<BrandUpsertRequestDto> upsertValidator)
    {
        _context = context;
        _photoService = photoService;
        _authorizationService = authorizationService;
        _listQueryService = listQueryService;
        _exceptionHandler = exceptionHandler;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<BrandListResponseDto> GetListAsync(
            BrandListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<Brand> query = _context.Brands.OrderBy(b => b.Id);

        return await _listQueryService.GetPagedListAsync(
            query,
            requestDto,
            (entities, pageCount) => new BrandListResponseDto(entities, pageCount),
            cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<List<BrandMinimalResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Brands
            .OrderBy(brand => brand.Id)
            .Select(brand => new BrandMinimalResponseDto(brand))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<BrandDetailResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Brand brand = await _context.Brands
            .Include(b => b.Country)
            .Where(b => b.Id == id)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException();

        BrandExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetExistingAuthorization<Brand, BrandExistingAuthorizationResponseDto>();

        return new BrandDetailResponseDto(brand, authorizationResponseDto);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(BrandUpsertRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _upsertValidator.ValidateAndThrow(requestDto);

        Brand brand = new Brand
        {
            Name = requestDto.Name,
            Website = requestDto.Website,
            SocialMediaUrl = requestDto.SocialMediaUrl,
            PhoneNumber = requestDto.PhoneNumber,
            Email = requestDto.Email,
            Address = requestDto.Address,
            CountryId = requestDto.CountryId
        };

        if (requestDto.Photos.Count > 0)
        {
            await _photoService.CreateMultipleAsync(brand, requestDto.Photos, cancellationToken);
        }

        try
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync(cancellationToken);

            return brand.Id;
        }
        catch (DbUpdateException exception)
        {
            foreach (Photo photo in brand.Photos)
            {
                _photoService.Delete(photo.Url);
            }

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
            BrandUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _upsertValidator.ValidateAndThrow(requestDto);

        Brand brand = await _context.Brands.SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        // Update the other properties.
        brand.Name = requestDto.Name;
        brand.Website = requestDto.Website;
        brand.SocialMediaUrl = requestDto.SocialMediaUrl;
        brand.PhoneNumber = requestDto.PhoneNumber;
        brand.Email = requestDto.Email;
        brand.Address = requestDto.Address;

        List<string> urlsToBeDeletedWhenSuccessful;
        List<string> urlsToBeDeletedWhenFailed;
        (urlsToBeDeletedWhenSuccessful, urlsToBeDeletedWhenFailed) = await _photoService.UpdateMultipleAsync(
            brand,
            requestDto.Photos,
            cancellationToken
        );

        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            // The brand entity has been updated successfully, delete the old photos.
            foreach (string url in urlsToBeDeletedWhenSuccessful)
            {
                _photoService.Delete(url);
            }
        }
        catch (DbUpdateException exception)
        {
            foreach (string url in urlsToBeDeletedWhenFailed)
            {
                _photoService.Delete(url);
            }

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
        Brand brand = await _context.Brands
            .Include(b => b.Photos)
            .SingleOrDefaultAsync(b => b.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (string url in brand.Photos.Select(p => p.Url))
        {
            _photoService.Delete(url);
        }
    }

    /// <inheritdoc />
    public ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions = new()
        {
            new()
            {
                Name = nameof(BrandListRequestDto.FieldToSort.CreatedDateTime),
                DisplayName = DisplayNames.CreatedDateTime
            }
        };

        return new()
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions.Select(fo => fo.Name).Single(),
            DefaultAscending = true
        };
    }

    /// <inheritdoc />
    public bool GetCreatingPermission()
    {
        return _authorizationService.CanCreate<Brand>();
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

        if (handledResult.IsForeignKeyConstraintViolation)
        {
            return OperationException.NotFound(
                new object[] { nameof(BrandUpsertRequestDto.CountryId) },
                DisplayNames.Country
            );
        }

        if (handledResult.IsUniqueConstraintViolation)
        {
            return OperationException.Duplicated(
                new object[] { nameof(BrandUpsertRequestDto.Name) },
                DisplayNames.Name
            );
        }

        return null;
    }
    #endregion
}
