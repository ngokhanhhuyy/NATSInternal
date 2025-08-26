namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class AnnouncementService : IAnnouncementService
{
    #region Fields
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IListQueryService _listQueryService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<AnnouncementListRequestDto> _listValidator;
    private readonly IValidator<AnnouncementUpsertRequestDto> _upsertValidator;
    #endregion

    #region Constructors
    public AnnouncementService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IListQueryService listQueryService,
            IDbExceptionHandler exceptionHandler,
            IValidator<AnnouncementListRequestDto> listValidator,
            IValidator<AnnouncementUpsertRequestDto> upsertValidator)
    {
        _context = context;
        _authorizationService = authorizationService;
        _listQueryService = listQueryService;
        _exceptionHandler = exceptionHandler;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<AnnouncementListResponseDto> GetListAsync(
            AnnouncementListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _listValidator.ValidateAndThrow(requestDto);
        
        // Initialize query statement.
        IQueryable<Announcement> query = _context.Announcements
            .Include(a => a.CreatedUser)
            .OrderByDescending(a => a.StartingDateTime)
                .ThenByDescending(a => a.EndingDateTime)
                .ThenByDescending(a => a.CreatedDateTime);

        return await _listQueryService.GetPagedListAsync(
            query,
            requestDto,
            (announcement) =>
            {
                AnnouncementExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
                    .GetExistingAuthorization<Announcement, AnnouncementExistingAuthorizationResponseDto>();

                return new AnnouncementResponseDto(announcement, authorizationResponseDto);
            },
            (announcementResponseDtos, pageCount) =>
            {
                return new AnnouncementListResponseDto(announcementResponseDtos, pageCount);
            },
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<AnnouncementResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Announcement announcement = await _context.Announcements
            .Include(a => a.CreatedUser)
            .SingleOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        AnnouncementExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetExistingAuthorization<Announcement, AnnouncementExistingAuthorizationResponseDto>();

        return new(announcement, authorizationResponseDto);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(
            AnnouncementUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet();
        });

        // Initialize the entity.
        DateTime startingDateTime = requestDto.StartingDateTime ?? DateTime.UtcNow.ToApplicationTime();
        Announcement announcement = new Announcement
        {
            Category = requestDto.Category,
            Title = requestDto.Title,
            Content = requestDto.Content,
            StartingDateTime = startingDateTime,
            EndingDateTime = startingDateTime.AddMinutes(requestDto.IntervalInMinutes),
            CreatedUserId = _authorizationService.GetUserId()
        };

        _context.Announcements.Add(announcement);

        // Save changes.
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return announcement.Id;
        }
        catch (DbUpdateException exception)
        {
            ConcurrencyException? convertedException = TryConvertDbUpdateException(exception);
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
            AnnouncementUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Update").IncludeRulesNotInRuleSet();
        });

        // Fetch the entity from the database and ensure it exists.
        Announcement announcement = await _context.Announcements
            .SingleOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        // Updating the entity's properties.
        announcement.Category = requestDto.Category;
        announcement.Title = requestDto.Title;
        announcement.Content = requestDto.Content;
        
        if (requestDto.StartingDateTime.HasValue)
        {
            announcement.StartingDateTime = requestDto.StartingDateTime ?? DateTime.UtcNow.ToApplicationTime();
            announcement.EndingDateTime = announcement.StartingDateTime.AddMinutes(requestDto.IntervalInMinutes); 
        }
        
        // Save changes.
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            ConcurrencyException? convertedException = TryConvertDbUpdateException(exception);
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
        int affectedRows = await _context.Announcements.Where(a => a.Id == id).ExecuteDeleteAsync(cancellationToken);
        if (affectedRows == 0)
        {
            throw new NotFoundException();
        }
    }
    
    /// <inheritdoc />
    public ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions = new()
        {
            new()
            {
                Name = nameof(OrderByFieldOption.StartingDateTime),
                DisplayName = DisplayNames.StartingDateTime
            }
        };

        return new()
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions.Single().Name,
            DefaultAscending = false
        };
    }

    /// <inheritdoc />
    public bool GetCreatingPermission()
    {
        return _authorizationService.CanCreate<Announcement>();
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Convert the exception which is thrown by the database during the creating or updating operation into an instance
    /// of <see cref="ConcurrencyException"/> .
    /// </summary>
    /// <param name="exception">
    /// An instance of the <see cref="DbUpdateException"/> class, contanining the details of the error.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="ConcurrencyException"/> class, representing the converted exception (when
    /// successful) or <see langword="null"/> (if failure).
    /// </returns>
    private ConcurrencyException? TryConvertDbUpdateException(DbUpdateException exception)
    {
        DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
        if (handledResult is not null &&
            (handledResult.IsConcurrencyConflict || handledResult.IsForeignKeyConstraintViolation))
        {
            return new ConcurrencyException();
        }

        return null;
    }
    #endregion
}