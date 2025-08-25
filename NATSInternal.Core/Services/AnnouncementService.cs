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
    #endregion

    #region Constructors
    public AnnouncementService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IListQueryService listQueryService,
            IDbExceptionHandler exceptionHandler,
            IValidator<AnnouncementListRequestDto> listValidator)
    {
        _context = context;
        _authorizationService = authorizationService;
        _listQueryService = listQueryService;
        _exceptionHandler = exceptionHandler;
        _listValidator = listValidator;
    }
    #endregion

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

        // Perform the creating opeartion.
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return announcement.Id;
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlException)
        {
            TryConvertDbUpdateException(sqlException);

            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(
            Guid id,
            AnnouncementUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
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
        when (exception.InnerException is MySqlException sqlException)
        {
            TryConvertDbUpdateException(sqlException);

            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        int affectedRows = await _context.Announcements
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            throw new NotFoundException();
        }
    }
    
    /// <inheritdoc cref="IAnnouncementService.GetListSortingOptions" />
    public override ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions;
        fieldOptions = new List<ListSortingByFieldResponseDto>
        {
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.StartingDateTime),
                DisplayName = DisplayNames.StartingDateTime
            }
        };

        return new ListSortingOptionsResponseDto
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions.Single().Name,
            DefaultAscending = false
        };
    }

    /// <summary>
    /// Handle the exception thrown from the database during the creating or updating operation and convert it into the
    /// appropriate exception.
    /// </summary>
    /// <param name="exception">
    /// The exception thrown by the database.
    /// </param>
    /// <exception cref="ConcurrencyException">
    /// Thrown when there is some concurrent conflict during the operation.
    /// </exception>
    private static void TryConvertDbUpdateException(MySqlException exception)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(exception);
        if (exceptionHandler.IsForeignKeyNotFound)
        {
            throw new ConcurrencyException();
        }
    }
}