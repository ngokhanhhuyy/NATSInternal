using System.Data.Common;

namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class UserService : IUserService
{
    #region Fields
    private readonly DatabaseContext _context;
    private readonly IListQueryService _listQueryService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<UserListRequestDto> _listValidator;
    private readonly IValidator<UserCreateRequestDto> _createValidator;
    private readonly IValidator<UserAddToRolesRequestDto> _addToRolesValidator;
    private readonly IValidator<UserRemoveFromRolesRequestDto> _removeFromRolesValidator;
    private readonly IValidator<UserPasswordChangeRequestDto> _passwordChangeValidator;
    private readonly IValidator<UserPasswordResetRequestDto> _passwordResetValidator;
    #endregion

    #region Constructors
    public UserService(
            DatabaseContext context,
            IListQueryService listQueryService,
            IPasswordHashingService passwordHashingService,
            IAuthorizationInternalService authorizationService,
            IDbExceptionHandler exceptionHandler,
            IValidator<UserListRequestDto> listValidator,
            IValidator<UserCreateRequestDto> createValidator,
            IValidator<UserAddToRolesRequestDto> addToRolesValidator,
            IValidator<UserRemoveFromRolesRequestDto> removeFromRolesValidator,
            IValidator<UserPasswordChangeRequestDto> passwordChangeValidator,
            IValidator<UserPasswordResetRequestDto> passwordResetValidator)
    {
        _context = context;
        _listQueryService = listQueryService;
        _passwordHashingService = passwordHashingService;
        _authorizationService = authorizationService;
        _exceptionHandler = exceptionHandler;
        _listValidator = listValidator;
        _createValidator = createValidator;
        _addToRolesValidator = addToRolesValidator;
        _removeFromRolesValidator = removeFromRolesValidator;
        _passwordChangeValidator = passwordChangeValidator;
        _passwordResetValidator = passwordResetValidator;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<UserListResponseDto> GetListAsync(
            UserListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the request.
        requestDto.TransformValues();
        _listValidator.ValidateAndThrow(requestDto);
        
        // Initialize query.
        IQueryable<User> query = _context.Users.Include(u => u.Roles);

        // Determine the field and the direction the sort.
        string sortingByField = requestDto.SortingByFieldName ?? GetListSortingOptions().DefaultFieldName;
        bool sortingByAscending = requestDto.SortingByAscending ?? GetListSortingOptions().DefaultAscending;
        switch (sortingByField)
        {
            case nameof(UserListRequestDto.FieldToSort.UserName) or null:
                query = _listQueryService.ApplySorting(query, u => u.UserName, sortingByAscending);
                break;
            case nameof(UserListRequestDto.FieldToSort.RoleMaxPowerLevel):
                query = _listQueryService.ApplySorting(query, u => u.Roles.Max(r => r.PowerLevel), sortingByAscending);
                break;
            case nameof(OrderByFieldOption.CreatedDateTime):
                query = _listQueryService.ApplySorting(query, u => u.CreatedDateTime, sortingByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        // Filter by role.
        if (requestDto.RoleId.HasValue)
        {
            query = query.Where(u => u.Roles.Select(r => r.Id).Contains(requestDto.RoleId.Value));
        }

        // Filter by search content.
        if (requestDto.SearchContent != null && requestDto.SearchContent.Length >= 3)
        {
            query = query.Where(u => u.NormalizedUserName.Contains(requestDto.SearchContent));
        }

        return await _listQueryService.GetPagedListAsync(
            query,
            requestDto,
            (entities, pageCount) => new UserListResponseDto(entities, pageCount),
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<UserBasicResponseDto>> GetMultipleAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default)
    {
        // Fetch a list of users with specified ids.
        List<User> users = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Permissions)
            .OrderBy(u => u.Id)
            .Where(u => ids.Contains(u.Id))
            .ToListAsync(cancellationToken);

        // Ensure all of the ids exist.
        if (ids.Except(users.Select(u => u.Id)).Any())
        {
            throw new NotFoundException();
        }

        return users.Select(u => new UserBasicResponseDto(u)).ToList();
    }

    /// <inheritdoc />
    public async Task<RoleDetailResponseDto> GetRoleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .Include(ur => ur.Role).ThenInclude(r => r.Permissions)
            .Where(ur => ur.UserId == id)
            .Select(ur => new RoleDetailResponseDto(ur.Role))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException();
    }

    /// <inheritdoc />
    public async Task<UserBasicResponseDto> GetBasicAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Permissions)
            .Where(u => u.Id == id)
            .Select(u => new UserBasicResponseDto(u))
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException();
    }

    /// <inheritdoc />
    public async Task<UserDetailResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User user = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Permissions)
            .SingleOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken)
            ?? throw new NotFoundException();

        return new UserDetailResponseDto(user, _authorizationService.GetUserDetailAuthorization(user));
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(UserCreateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _createValidator.ValidateAndThrow(requestDto);

        // Initialize the entity.
        User user = new User
        {
            UserName = requestDto.UserName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(requestDto.Password)
        };

        _context.Users.Add(user);

        if (requestDto.RoleNames.Count > 0)
        {
            // Fetch the requested roles by names.
            List<Role> roles = await _context.Roles
                .Where(r => requestDto.RoleNames.Contains(r.Name))
                .ToListAsync(cancellationToken);

            // Ensure all requested roles exist.
            List<string> existingRoleNames = roles.Select(r => r.Name).ToList();
            for (int i = 0; i < requestDto.RoleNames.Count; i++)
            {
                Role? role = roles
                    .SingleOrDefault(r => r.Name == requestDto.RoleNames[i])
                    ?? throw OperationException.NotFound(
                        new object[] { nameof(requestDto.RoleNames), i },
                        DisplayNames.Role
                    );

                user.Roles.Add(role);
            }
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsUniqueConstraintViolation)
            {
                throw OperationException.Duplicated(
                    new object[] { nameof(requestDto.UserName) },
                    DisplayNames.UserName
                );
            }

            if (handledResult.IsForeignKeyConstraintViolation || handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task AddToRolesAsync(
            Guid id,
            UserAddToRolesRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _addToRolesValidator.ValidateAndThrow(requestDto);

        User user = await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        List<Role> existingRoles = await _context.Roles
            .Where(r => requestDto.RoleNames.Contains(r.Name))
            .ToListAsync(cancellationToken);

        List<string> addedRoleNames = user.Roles.Select(r => r.Name).ToList();

        for (int i = 0; i < existingRoles.Count; i++)
        {
            if (!requestDto.RoleNames.Contains(existingRoles[i].Name))
            {
                throw OperationException.NotFound(
                    new object[] { nameof(requestDto.RoleNames), i },
                    DisplayNames.Role
                );
            }

            if (addedRoleNames.Contains(existingRoles[i].Name))
            {
                throw new OperationException(
                    new object[] { nameof(requestDto.RoleNames), i },
                    ErrorMessages.UserAlreadyInRole.Replace("{RoleName}", existingRoles[i].DisplayName)
                );
            }

            user.Roles.Add(existingRoles[i]);
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is not null &&
                (handledResult.IsForeignKeyConstraintViolation || handledResult.IsConcurrencyConflict))
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task RemoveFromRolesAsync(
            Guid id,
            UserRemoveFromRolesRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _removeFromRolesValidator.ValidateAndThrow(requestDto);

        User user = await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        for (int i = 0; i < requestDto.RoleNames.Count; i++)
        {
            string roleName = requestDto.RoleNames[i];
            Role role = user.Roles
                .SingleOrDefault(r => r.Name == roleName)
                ?? throw new OperationException(
                    new object[] { nameof(requestDto.RoleNames), i },
                    ErrorMessages.UserNotInRole.Replace("{RoleName}", roleName));

            user.Roles.Remove(role);
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is not null && handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task ChangePasswordAsync(
            UserPasswordChangeRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _passwordChangeValidator.ValidateAndThrow(requestDto);

        // Fetch the entity with given id and ensure the entity exists.
        Guid id = _authorizationService.GetUserId();
        User user = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        // Ensure having permission to change password of the fetched user.
        if (!_authorizationService.CanChangeUserPassword(user))
        {
            throw new AuthorizationException();
        }

        // Verify current password.
        bool isPasswordCorrect = _passwordHashingService.VerifyPassword(requestDto.CurrentPassword, user.PasswordHash);
        if (!isPasswordCorrect)
        {
            throw new OperationException(
                new object[] { nameof(requestDto.CurrentPassword) },
                ErrorMessages.Incorrect.ReplacePropertyName(DisplayNames.CurrentPassword)
            );
        }

        // Generate new password hash.
        user.PasswordHash = _passwordHashingService.HashPassword(requestDto.NewPassword);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task ResetPasswordAsync(
            Guid id,
            UserPasswordResetRequestDto requestDto,
            CancellationToken cancellationToken)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _passwordResetValidator.ValidateAndThrow(requestDto);
        
        // Fetch the entity with given id and ensure the entity exists.
        User user = await _context.Users
            .Include(u => u.Roles).ThenInclude(u => u.Permissions)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        // Check if having permission to reset password of the fetched user.
        if (!_authorizationService.CanResetUserPassword(user))
        {
            throw new AuthorizationException();
        }

        // Performing password reset operation.
        user.PasswordHash = _passwordHashingService.HashPassword(requestDto.NewPassword);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Prepare the query.
        IQueryable<User> query = _context.Users.Where(u => u.Id == id);

        // Fetch the user entity with given id from the database and ensure the entity exists.
        User user = await query
            .Include(u => u.Roles).ThenInclude(r => r.Permissions)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException();

        // Ensure having permission to delete the user.
        if (!_authorizationService.CanDeleteUser(user))
        {
            throw new AuthorizationException();
        }

        // Performing deleting operation.
        bool deletionSucceeded = await ExecuteDeleteWithDbExceptionHandlingAsync(
            async (token) => await query.ExecuteDeleteAsync(token),
            true,
            cancellationToken
        );

        if (deletionSucceeded)
        {
            return;
        }
        
        await ExecuteDeleteWithDbExceptionHandlingAsync(
            async (token) => await query.ExecuteUpdateAsync(
                setters => setters.SetProperty(u => u.IsDeleted, true),
                cancellationToken
            ),
            false,
            cancellationToken
        );
    }

    /// <inheritdoc />
    public ListSortingOptionsResponseDto GetListSortingOptions()
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
                Name = nameof(OrderByFieldOption.UserName),
                DisplayName = DisplayNames.UserName
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.Birthday),
                DisplayName = DisplayNames.Birthday
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.Age),
                DisplayName = DisplayNames.Age
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.CreatedDateTime),
                DisplayName = DisplayNames.CreatedDateTime
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.Role),
                DisplayName = DisplayNames.Role
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
            DefaultFieldName = fieldOptions.Single(i => i.Name == nameof(OrderByFieldOption.LastName)).Name,
            DefaultAscending = true
        };
    }

    /// <inheritdoc />
    public bool GetCreatingPermission()
    {
        return _authorizationService.CanCreateUser();
    }

    /// <inheritdoc />
    public async Task<bool> GetPasswordResetPermission(Guid id, CancellationToken cancellationToken = default)
    {
        User user = await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        return _authorizationService.CanResetUserPassword(user);
    }
    #endregion

    #region PrivateMethods
    private async Task<bool> ExecuteDeleteWithDbExceptionHandlingAsync(
            Func<CancellationToken, Task> action,
            bool handleForeignKeyConstraintViolation,
            CancellationToken cancellationToken = default)
    {
        try
        {
            await action(cancellationToken);
            return true;
        }
        catch (DbException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            if (!handledResult.IsForeignKeyConstraintViolation)
            {
                throw;
            }

            if (handleForeignKeyConstraintViolation)
            {
                return false;
            }


            throw OperationException.DeleteRestricted(Array.Empty<object>(), DisplayNames.User);
        }
    }
    #endregion
}