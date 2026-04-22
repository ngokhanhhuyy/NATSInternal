namespace NATSInternal.Core.Services;

/// <inheritdoc cref="INotificationService" />
internal class NotificationService
    :
        UpsertableAbstractService<
            Notification,
            NotificationListRequestDto,
            NotificationExistingAuthorizationResponseDto>,
        INotificationService
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;

    public NotificationService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService) : base(authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public async Task<NotificationListResponseDto> GetListAsync(
            NotificationListRequestDto requestDto = null)
    {
        requestDto ??= new NotificationListRequestDto();

        // Initialize query.
        int currentUserId = _authorizationService.GetUserId();
        IQueryable<Notification> query = _context.Notifications
            .Include(n => n.CreatedUser).ThenInclude(u => u.Roles)
            .Include(n => n.ReadUsers).ThenInclude(u => u.Roles)
            .Include(n => n.ReadUsers).ThenInclude(u => u.ReceivedNotifications)
            .OrderByDescending(n => n.CreatedDateTime)
            .Where(n => n.ReceivedUsers.Select(u => u.Id).Contains(currentUserId));

        // Filter by unread notifications only.
        if (requestDto.UnreadOnly)
        {
            query = query.Where(n => !n.ReadUsers.Select(u => u.Id).Contains(currentUserId));
        }

        // Fetch the list of the entities.
        EntityListDto<Notification> listDto = await GetListOfEntitiesAsync(query, requestDto);

        return new NotificationListResponseDto
        {
            PageCount = listDto.PageCount,
            Items = listDto.Items?
                .Select(notification => new NotificationResponseDto(
                    notification,
                    currentUserId))
                .ToList()
        };
    }

    public async Task<NotificationResponseDto> GetSingleAsync(int id)
    {
        int currentUserId = _authorizationService.GetUserId();
        return await _context.Notifications
            .Include(n => n.CreatedUser)
            .Include(n => n.ReadUsers).ThenInclude(u => u.ReceivedNotifications)
            .OrderBy(n => n.Id)
            .Where(n => n.ReceivedUsers.Select(u => u.Id).Contains(currentUserId))
            .Where(n => n.Id == id)
            .Select(n => new NotificationResponseDto(n, currentUserId))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    /// <inheritdoc />
    public async Task MarkAsReadAsync(int id)
    {
        // Fetch the current user entity.
        User currentUser = await GetCurrentUser();

        // Fetch the notification entity.
        Notification notification = await _context.Notifications
            .Include(n => n.ReceivedUsers)
            .Include(n => n.ReadUsers)
            .Where(n => n.Id == id)
            .Where(n => n.ReceivedUsers.Select(u => u.Id).Contains(currentUser.Id))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();

        // Add the current user to the notification's read user list.
        if (!notification.ReadUsers.Select(u => u.Id).Contains(currentUser.Id))
        {
            notification.ReadUsers.Add(currentUser);

            // Save the changes.
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task MarkAllAsReadAsync()
    {
        // Fetch the current user entity.
        User currentUser = await GetCurrentUser();

        // Fetch the notification entity.
        List<Notification> notifications = await _context.Notifications
            .Include(n => n.ReceivedUsers)
            .Include(n => n.ReadUsers)
            .Where(n => n.ReceivedUsers.Select(u => u.Id).Contains(currentUser.Id))
            .Where(n => !n.ReadUsers.Select(u => u.Id).Contains(currentUser.Id))
            .ToListAsync();

        // Add the current user to each notification's read user list.
        foreach (Notification notification in notifications)
        {
            if (!notification.ReadUsers.Select(u => u.Id).Contains(currentUser.Id))
            {
                notification.ReadUsers.Add(currentUser);

                // Save the changes.
                await _context.SaveChangesAsync();
            }
        }
    }
    /// <inheritdoc />
    public async Task<(List<int>, int)> CreateAsync(
            NotificationType type,
            List<int> resourceIds)
    {
        // Fetch the list of all users' ids.
        List<int> userIds = await _context.Users
            .Where(u => !u.IsDeleted)
            .Select(u => u.Id)
            .ToListAsync();

        NotificationType[] selfCreatedNotificationType =
        {
            NotificationType.UserBirthday,
            NotificationType.UserJoiningDateAnniversary,
            NotificationType.CustomerBirthday
        };

        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine the interacted user id value.
        int? createdUserId = null;
        if (!selfCreatedNotificationType.Contains(type))
        {
            createdUserId = _authorizationService.GetUserId();
        }

        // Initialize the entity.
        Notification notification = new Notification
        {
            Type = type,
            CreatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            ResourceIds = resourceIds,
            CreatedUserId = createdUserId
        };
        
        _context.Notifications.Add(notification);

        // Initialize the relationship between all users (as notification receivers)
        // and the notification.
        foreach (int userId in userIds)
        {
            NotificationReceivedUser notificationReceivedUser;
            notificationReceivedUser = new NotificationReceivedUser
            {
                ReceivedNotification = notification,
                ReceivedUserId = userId
            };
            _context.NotificationReceivedUsers.Add(notificationReceivedUser);
        }
        // Save the notification received user entity.
        await _context.SaveChangesAsync();

        // Commit the transaction and finish the operation.
        await transaction.CommitAsync();
        return (userIds, notification.Id);
    }
    
    /// <inheritdoc cref="INotificationService.GetListSortingOptions" />
    public override ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions;
        fieldOptions = new List<ListSortingByFieldResponseDto>
        {
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.FirstName),
                DisplayName = DisplayNames.FirstName
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.Birthday),
                DisplayName = DisplayNames.Birthday
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.CreatedDateTime),
                DisplayName = DisplayNames.CreatedDateTime
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.DebtRemainingAmount),
                DisplayName = DisplayNames.DebtRemainingAmount
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.LastName),
                DisplayName = DisplayNames.LastName
            }
        };

        return new ListSortingOptionsResponseDto
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions
                .Single(i => i.Name == nameof(OrderByFieldOption.LastName))
                .Name,
            DefaultAscending = true
        };
    }

    /// <summary>
    /// Fetch the entity of the current user.
    /// </summary>
    /// <returns>The entity of the current user.</returns>
    private async Task<User> GetCurrentUser()
    {
        int currentUserId = _authorizationService.GetUserId();
        return await _context.Users
            .SingleOrDefaultAsync(u => u.Id == currentUserId);
    }
}