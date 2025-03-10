﻿namespace NATSInternal.Services;

/// <inheritdoc />
internal class UserService : IUserService
{
    private readonly DatabaseContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly ISinglePhotoService<User> _photoService;

    public UserService(
            DatabaseContext context,
            UserManager<User> userManager,
            IAuthorizationInternalService authorizationService,
            ISinglePhotoService<User> photoService)
    {
        _context = context;
        _userManager = userManager;
        _authorizationService = authorizationService;
        _photoService = photoService;
    }

    /// <inheritdoc />
    public async Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto)
    {
        // Initialize query.
        IQueryable<User> query = _context.Users
            .Include(u => u.Roles)
            .Where(u => !u.IsDeleted);
        // Determine the field and the direction the sort.
        string sortingByField = requestDto.SortingByField
                                ?? GetListSortingOptions().DefaultFieldName;
        bool sortingByAscending = requestDto.SortingByAscending
                                  ?? GetListSortingOptions().DefaultAscending;
        switch (sortingByField)
        {
            case nameof(OrderByFieldOption.FirstName):
                query = sortingByAscending
                    ? query.OrderBy(u => u.FirstName)
                    : query.OrderByDescending(u => u.FirstName);
                break;
            case nameof(OrderByFieldOption.UserName):
                query = sortingByAscending
                    ? query.OrderBy(u => u.UserName)
                    : query.OrderByDescending(u => u.UserName);
                break;
            case nameof(OrderByFieldOption.Birthday):
                query = sortingByAscending
                    ? query.OrderBy(u => u.Birthday.Value.Month)
                        .ThenBy(u => u.Birthday.Value.Day)
                    : query.OrderByDescending(u => u.Birthday.Value.Month)
                        .ThenByDescending(u => u.Birthday.Value.Day);
                break;
            case nameof(OrderByFieldOption.Age):
                query = sortingByAscending
                    ? query.OrderBy(u => u.Birthday)
                    : query.OrderByDescending(u => u.Birthday);
                break;
            case nameof(OrderByFieldOption.CreatedDateTime):
                query = sortingByAscending
                    ? query.OrderBy(u => u.CreatedDateTime)
                    : query.OrderByDescending(u => u.CreatedDateTime);
                break;
            case nameof(OrderByFieldOption.Role):
                query = sortingByAscending
                    ? query.OrderBy(u => u.Roles.First().Id)
                    : query.OrderByDescending(u => u.Roles.First().Id);
                break;
            case nameof(OrderByFieldOption.LastName):
                query = sortingByAscending
                    ? query.OrderBy(u => u.LastName)
                    : query.OrderByDescending(u => u.LastName);
                break;
            default:
                throw new NotImplementedException();
        }

        // Filter by role.
        if (requestDto.RoleId.HasValue)
        {
            query = query
                .Where(u => u.Roles.First().Id == requestDto.RoleId.Value);
        }

        // Filter by joined recently.
        if (requestDto.JoinedRencentlyOnly)
        {
            DateOnly minimumJoiningDate = DateOnly
                .FromDateTime(DateTime.UtcNow.ToApplicationTime())
                .AddMonths(-1);
            query = query
                .Where(u => u.JoiningDate.HasValue)
                .Where(u => u.JoiningDate.Value > minimumJoiningDate);
        }

        // Filter by having incoming birthday.
        if (requestDto.UpcomingBirthdayOnly)
        {
            DateOnly minRange = DateOnly.FromDateTime(DateTime.Today);
            DateOnly maxRange = minRange.AddMonths(1);
            int thisYear = DateTime.Today.Year;
            query = query
                .Where(u => u.Birthday.HasValue && !u.IsDeleted)
                .Where(u =>
                    (
                        minRange <= u.Birthday.Value
                            .AddYears(thisYear - u.Birthday.Value.Year) &&
                        maxRange > u.Birthday.Value
                            .AddYears(thisYear - u.Birthday.Value.Year)
                    ) || (
                        minRange <= u.Birthday.Value
                            .AddYears(thisYear - u.Birthday.Value.Year + 1) &&
                        maxRange > u.Birthday.Value
                            .AddYears(thisYear - u.Birthday.Value.Year + 1)
                    ));
        }

        // Filter by search content.
        if (requestDto.Content != null && requestDto.Content.Length >= 3)
        {
            query = query
                .Where(u =>
                    u.NormalizedFullName.Contains(requestDto.Content) ||
                    u.NormalizedUserName.Contains(requestDto.Content.ToUpper()));
        }

        // Initialize response dto.
        UserListResponseDto responseDto = new UserListResponseDto();
        int resultCount = await query.CountAsync();
        if (resultCount == 0)
        {
            responseDto.PageCount = 0;
            return responseDto;
        }
        responseDto.PageCount = (int)Math.Ceiling(
            (double)resultCount / requestDto.ResultsPerPage);
        responseDto.Items = await query
            .Skip(requestDto.ResultsPerPage * (requestDto.Page - 1))
            .Take(requestDto.ResultsPerPage)
            .Select(u => new UserBasicResponseDto(
                u,
                _authorizationService.GetUserBasicAuthorization(u)))
            .ToListAsync()
            ?? new List<UserBasicResponseDto>();

        return responseDto;
    }

    /// <inheritdoc />
    public async Task<List<UserBasicResponseDto>> GetMultipleAsync(IEnumerable<int> ids)
    {
        // Fetch a list of users with specified ids.
        List<User> users = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .OrderBy(u => u.Id)
            .Where(u => ids.Contains(u.Id))
            .ToListAsync();

        // Ensure all of the ids exist.
        if (ids.Except(users.Select(u => u.Id)).Any())
        {
            throw new ResourceNotFoundException();
        }

        return users.Select(u => new UserBasicResponseDto(u)).ToList();
    }

    /// <inheritdoc />
    public async Task<UserListResponseDto> GetJoinedRecentlyListAsync()
    {
        DateOnly minimumJoiningDate = DateOnly
            .FromDateTime(DateTime.UtcNow.ToApplicationTime())
            .AddMonths(-1);

        List<User> users = await _context.Users
            .Include(u => u.Roles)
            .OrderBy(u => u.JoiningDate)
            .Where(u => u.JoiningDate.HasValue && !u.IsDeleted)
            .Where(u => u.JoiningDate.Value > minimumJoiningDate)
            .ToListAsync();

        return new UserListResponseDto
        {
            Items = users
                .Select(u => new UserBasicResponseDto(u))
                .ToList()
        };
    }

    /// <inheritdoc />
    public async Task<UserListResponseDto> GetUpcomingBirthdayListAsync()
    {
        DateOnly minRange = DateOnly.FromDateTime(DateTime.Today);
        DateOnly maxRange = minRange.AddMonths(1);
        int thisYear = DateTime.Today.Year;
        List<User> users = await _context.Users
            .Include(u => u.Roles)
            .OrderBy(u => u.Birthday.Value.Month).ThenBy(u => u.Birthday.Value.Day)
            .Where(u => u.Birthday.HasValue && !u.IsDeleted)
            .Where(u =>
                (
                    minRange <= u.Birthday.Value.AddYears(thisYear - u.Birthday.Value.Year) &&
                    maxRange > u.Birthday.Value.AddYears(thisYear - u.Birthday.Value.Year)
                ) || (
                    minRange <= u.Birthday.Value.AddYears(thisYear - u.Birthday.Value.Year + 1) &&
                    maxRange > u.Birthday.Value.AddYears(thisYear - u.Birthday.Value.Year + 1)
                )
            ).ToListAsync();

        return new UserListResponseDto
        {
            Items = users.Select(u => new UserBasicResponseDto(u)).ToList()
        };
    }

    /// <inheritdoc />
    public async Task<RoleDetailResponseDto> GetRoleAsync(int id)
    {
        RoleDetailResponseDto responseDto = await _context.UserRoles
            .Include(ur => ur.Role).ThenInclude(r => r.Claims)
            .Where(ur => ur.UserId == id)
            .Select(ur => new RoleDetailResponseDto(ur.Role)).SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());
        return responseDto;
    }

    /// <inheritdoc />
    public async Task<UserBasicResponseDto> GetBasicAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .Where(u => u.Id == id)
            .Select(u => new UserBasicResponseDto(u))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();
    }

    /// <inheritdoc />
    public async Task<UserDetailResponseDto> GetDetailAsync(int id)
    {
        User user = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .SingleOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());

        return new UserDetailResponseDto(
            user,
            _authorizationService.GetUserDetailAuthorization(user));
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(UserCreateRequestDto requestDto)
    {
        string fullName = PersonNameUtility.GetFullNameFromNameElements(
            requestDto.PersonalInformation.FirstName,
            requestDto.PersonalInformation.MiddleName,
            requestDto.PersonalInformation.LastName);

        // Create user.
        User user = new User
        {
            UserName = requestDto.UserName,
            FirstName = requestDto.PersonalInformation.FirstName,
            NormalizedFirstName = requestDto.PersonalInformation.FirstName
                .ToNonDiacritics()
                .ToUpper(),
            MiddleName = requestDto.PersonalInformation.MiddleName,
            NormalizedMiddleName = requestDto.PersonalInformation.MiddleName?
                .ToNonDiacritics()
                .ToUpper(),
            LastName = requestDto.PersonalInformation.LastName,
            NormalizedLastName = requestDto.PersonalInformation.LastName
                .ToNonDiacritics()
                .ToUpper(),
            FullName = fullName,
            NormalizedFullName = fullName.ToNonDiacritics().ToUpper(),
            Gender = requestDto.PersonalInformation.Gender,
            Birthday = requestDto.PersonalInformation.Birthday,
            PhoneNumber = requestDto.PersonalInformation.PhoneNumber,
            Email = requestDto.PersonalInformation.Email,
            JoiningDate = requestDto.UserInformation.JoiningDate,
            Note = requestDto.UserInformation.Note,
        };

        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        IdentityResult creatingUserResult;
        try
        {
            creatingUserResult = await _userManager.CreateAsync(
                user,
                requestDto.Password);
        }
        catch (DbUpdateException exception)
        {
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
                if (exceptionHandler.IsUniqueConstraintViolated)
                {
                    throw new DuplicatedException(exceptionHandler.ViolatedFieldName);
                }
            }
            throw;
        }

        if (creatingUserResult.Succeeded)
        {
            // Checking if the role name from the request exist
            Role role = await _context.Roles
                .SingleOrDefaultAsync(r => r.Name == requestDto.UserInformation.RoleName)
                ?? throw new ResourceNotFoundException(
                    nameof(Role),
                    nameof(requestDto.UserInformation.RoleName),
                    requestDto.UserInformation.RoleName);

            // Ensure the desired role's power level cannot be greater than
            // the requested user's power level.
            if (!_authorizationService.CanAssignToRole(role))
            {
                throw new AuthorizationException();
            }

            // Adding user to role
            IdentityResult result = await _userManager.AddToRoleAsync(
            user,
            requestDto.UserInformation.RoleName);
            if (result.Errors.Any())
            {
                throw new InvalidOperationException(result.Errors
                    .Select(e => e.Description)
                    .First());
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Save avatar if included in the request.
            if (requestDto.PersonalInformation.AvatarFile != null)
            {
                user.AvatarUrl = await _photoService
                    .CreateAsync(requestDto.PersonalInformation.AvatarFile, true);
            }

            return user.Id;
        }

        if (creatingUserResult.Errors.Any(e => e.Code == "DuplicateUserName"))
        {
            throw new DuplicatedException(nameof(requestDto.UserName));
        }

        throw new InvalidOperationException(creatingUserResult.Errors
            .Select(error => error.Description)
            .First());
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, UserUpdateRequestDto requestDto)
    {
        // Fetch the entity from the database.
        User user = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .SingleOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
                ?? throw new ResourceNotFoundException(
                    nameof(User),
                    nameof(id),
                    id.ToString());

        // Use transaction for atomic cancellation if there is any error during the operation.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Edit personal information if user has right.
        if (_authorizationService.CanEditUserPersonalInformation(user)
                && requestDto.PersonalInformation != null)
        {
            // Update avatar when the request specified.
            if (requestDto.PersonalInformation.AvatarChanged)
            {
                // Delete old avatar if there is any.
                if (user.AvatarUrl != null)
                {
                    _photoService.Delete(user.AvatarUrl);
                    user.AvatarUrl = null;
                }

                // Create new avatar if the request data contains it.
                if (requestDto.PersonalInformation.AvatarFile != null)
                {
                    user.AvatarUrl = await _photoService
                        .CreateAsync(requestDto.PersonalInformation.AvatarFile, true);
                }
            }
            string fullName = PersonNameUtility.GetFullNameFromNameElements(
                requestDto.PersonalInformation.FirstName,
                requestDto.PersonalInformation.MiddleName,
                requestDto.PersonalInformation.LastName);
            user.FirstName = requestDto.PersonalInformation.FirstName;
            user.MiddleName = requestDto.PersonalInformation.MiddleName;
            user.LastName = requestDto.PersonalInformation.LastName;
            user.FullName = fullName;
            user.NormalizedFirstName = requestDto.PersonalInformation.LastName
                .ToNonDiacritics()
                .ToUpper();
            user.NormalizedMiddleName = requestDto.PersonalInformation.MiddleName?
                .ToNonDiacritics()
                .ToUpper();
            user.NormalizedLastName = requestDto.PersonalInformation.LastName
                .ToNonDiacritics()
                .ToUpper();
            user.NormalizedFullName = fullName.ToNonDiacritics().ToUpper();
            user.Gender = requestDto.PersonalInformation.Gender;
            user.Birthday = requestDto.PersonalInformation.Birthday;
            user.PhoneNumber = requestDto.PersonalInformation.PhoneNumber;
            user.Email = requestDto.PersonalInformation.Email;
            user.NormalizedEmail = requestDto.PersonalInformation.Email?
                .ToUpper();
        }
        else
        {
            throw new AuthorizationException();
        }

        // Edit user's user information if user has right.
        if (_authorizationService.CanEditUserUserInformation(user) &&
                requestDto.UserInformation != null)
        {
            user.JoiningDate = requestDto.UserInformation.JoiningDate;
            user.Note = requestDto.UserInformation.Note;

            // Update user's role if needed.
            if (requestDto.UserInformation.RoleName != user.Role.Name)
            {
                // Ensure the desired role's power level cannot be greater than
                // the requested user's power level.
                Role role = await _context.Roles
                    .SingleOrDefaultAsync(r => r.Name == requestDto.UserInformation.RoleName)
                    ?? throw new ResourceNotFoundException(
                        nameof(Role),
                        "role.name",
                        requestDto.UserInformation.RoleName);
                if (!_authorizationService.CanAssignToRole(role))
                {
                    throw new AuthorizationException();
                }

                user.Roles.Remove(user.Role);
                user.Roles.Add(role);
            }
        }

        user.UpdatedDateTime = DateTime.UtcNow.ToApplicationTime();

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    /// <inheritdoc />
    public async Task ChangePasswordAsync(UserPasswordChangeRequestDto requestDto)
    {
        // Fetch the entity with given id and ensure the entity exists.
        int id = _authorizationService.GetUserId();
        User user = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());

        // Ensure having permission to change password of the fetched user.
        if (!_authorizationService.CanChangeUserPassword(user))
        {
            throw new AuthorizationException();
        }

        // Performing password change operation.
        IdentityResult result = await _userManager
            .ChangePasswordAsync(user, requestDto.CurrentPassword, requestDto.NewPassword);

        // Ensure the operation succeeded.
        if (!result.Succeeded)
        {
            throw new OperationException(
                nameof(requestDto.CurrentPassword),
                result.Errors.First().Description);
        }
    }

    /// <inheritdoc />
    public async Task ResetPasswordAsync(
            int id,
            UserPasswordResetRequestDto requestDto)
    {
        // Fetch the entity with given id and ensure the entity exists.
        User user = await _context.Users
            .Include(u => u.Roles).ThenInclude(u => u.Claims)
            .SingleOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());

        // Check if having permission to reset password of the fetched user.
        if (!_authorizationService.CanResetUserPassword(user))
        {
            throw new AuthorizationException();
        }

        // Performing password reset operation.
        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        IdentityResult result = await _userManager
            .ResetPasswordAsync(user, token, requestDto.NewPassword);

        // Ensure the operation succeeded.
        if (!result.Succeeded)
        {
            throw new OperationException(
                requestDto.NewPassword,
                result.Errors.First().Description);
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the user entity with given id from the database and ensure the entity exists.
        User user = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .SingleOrDefaultAsync(u => u.Id == id && !u.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(User),
                nameof(id),
                id.ToString());

        // Ensure having permission to delete the user.
        if (!_authorizationService.CanDeleteUser(user))
        {
            throw new AuthorizationException();
        }

        // Performing deleting operation.
        user.IsDeleted = true;

        // Save changes.
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task RestoreAsync(int id)
    {
        // Fetch the user entity from the database and ensure the entity exists.
        User user = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == id && u.IsDeleted)
            ?? throw new ResourceNotFoundException(nameof(User), nameof(id), id.ToString());

        // Ensure having permission to restore the user.
        if (!_authorizationService.CanRestoreUser(user))
        {
            throw new AuthorizationException();
        }

        // Perform restoration operation.
        user.IsDeleted = false;

        // Save changes.
        await _context.SaveChangesAsync();
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
            DefaultFieldName = fieldOptions
                .Single(i => i.Name == nameof(OrderByFieldOption.LastName))
                .Name,
            DefaultAscending = true
        };
    }

    /// <inheritdoc />
    public bool GetCreatingPermission()
    {
        return _authorizationService.CanCreateUser();
    }

    /// <inheritdoc />
    public async Task<bool> GetPasswordResetPermission(int id)
    {
        User user = await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == id)
            ?? throw new ResourceNotFoundException();
        return _authorizationService.CanResetUserPassword(user);
    }
}