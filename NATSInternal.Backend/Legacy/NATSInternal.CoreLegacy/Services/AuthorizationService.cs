namespace NATSInternal.Core.Services;

/// <inheritdoc cref="IAuthorizationInternalService"/>
internal class AuthorizationService : IAuthorizationInternalService
{
    #region Fields
    private readonly DatabaseContext _context;
    private User? _user;
    #endregion

    #region Constructors
    public AuthorizationService(DatabaseContext context)
    {
        _context = context;
    }
    #endregion
    
    #region PrivateProeperties
    private User User => _user ?? throw new InvalidOperationException("User has not been loaded yet."); 
    #endregion

    #region Methods
    /// <inheritdoc cref="IAuthorizationService" />
    public async Task SetUserId(Guid id, CancellationToken cancellationToken = default)
    {
        _user = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Permissions)
            .SingleAsync(u => u.Id == id, cancellationToken);
    }
    
    /// <inheritdoc />
    public Guid GetUserId()
    {
        return User.Id;
    }

    /// <inheritdoc />
    public UserDetailResponseDto GetUserDetail()
    {
        return new(User);
    }

    // Authorization for users.
    public UserBasicAuthorizationResponseDto GetUserBasicAuthorization(User targetUser)
    {
        return new()
        {
            CanEdit = CanEditUserPersonalInformation(targetUser) || CanEditUserUserInformation(targetUser),
            CanChangePassword = CanChangeUserPassword(targetUser),
            CanResetPassword = CanResetUserPassword(targetUser),
            CanDelete = CanDeleteUser(targetUser)
        };
    }

    public UserDetailAuthorizationResponseDto GetUserDetailAuthorization(User targetUser)
    {
        return new()
        {
            CanGetNote = CanGetNote(targetUser.PowerLevel),
            CanEdit = CanEditUserPersonalInformation(targetUser) || CanEditUserUserInformation(targetUser),
            CanEditUserPersonalInformation = CanEditUserPersonalInformation(targetUser),
            CanEditUserUserInformation = CanEditUserUserInformation(targetUser),
            CanAssignRole = CanAssignRole(),
            CanChangePassword = CanChangeUserPassword(targetUser),
            CanResetPassword = CanResetUserPassword(targetUser),
            CanDelete = CanDeleteUser(targetUser)
        };
    }

    // Authorization for other resources.
    public TResponseDto GetCreatingAuthorization<TEntity, TData, TResponseDto>()
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class
            where TResponseDto : IHasStatsCreatingAuthorizationResponseDto, new()
    {
        return new()
        {
            CanSetStatsDateTime = CanSetStatsDateTimeWhenCreating<TEntity, TData>()
        };
    }

    /// <inheritdoc />
    public TResponseDto GetExistingAuthorization<TEntity, TResponseDto>()
            where TEntity : class, IUpsertableEntity<TEntity>
            where TResponseDto : IUpsertableExistingAuthorizationResponseDto, new()
    {
        return new TResponseDto
        {
            CanEdit = CanEdit<TEntity>(),
            CanDelete = CanDelete<TEntity>()
        };
    }

    public TResponseDto GetExistingAuthorization<TEntity, TData, TResponseDto>(
            TEntity entity)
                where TEntity : class, IHasStatsEntity<TEntity, TData>
                where TData : class
                where TResponseDto : IHasStatsExistingAuthorizationResponseDto, new()
    {
        return new TResponseDto
        {
            CanEdit = CanEdit<TEntity, TData>(entity),
            CanDelete = CanDelete<TEntity, TData>(entity),
            CanSetStatsDateTime = CanSetStatsDateTimeWhenEditing<TEntity, TData>(entity)
        };
    }

    // Permissions to interact with users.
    public bool CanCreateUser()
    {
        return User.HasPermission(PermissionNameConstants.CreateUser);
    }
    
    public bool CanEditUserPersonalInformation(User targetUser)
    {
        // Check permission when the user is editing himself.
        if (User.Id == targetUser.Id && User.HasPermission(PermissionNameConstants.EditSelfPersonalInformation))
        {
            return true;
        }

        // Check permission when the user is editing another user.
        if (User.HasPermission(PermissionNameConstants.EditOtherUserPersonalInformation) &&
            User.PowerLevel > targetUser.PowerLevel)
        {
            return true;
        }

        return false;
    }

    public bool CanEditUserUserInformation(User targetUser)
    {
        // Check permission when the user is editing himself.
        if (User.Id == targetUser.Id && User.HasPermission(PermissionNameConstants.EditSelfUserInformation))
        {
            return true;
        }

        // Check permission when the user is editing another user.
        if (User.HasPermission(PermissionNameConstants.EditOtherUserUserInformation) &&
            User.PowerLevel > targetUser.PowerLevel)
        {
            return true;
        }

        return false;
    }

    public bool CanChangeUserPassword(User targetUser)
    {
        return User.Id == targetUser.Id;
    }

    public bool CanResetUserPassword(User targetUser)
    {
        return User.Id != targetUser.Id &&
            User.HasPermission(PermissionNameConstants.ResetOtherUserPassword) &&
            User.PowerLevel > targetUser.PowerLevel;
    }

    public bool CanDeleteUser(User targetUser)
    {
        return User.Id != targetUser.Id &&
            User.HasPermission(PermissionNameConstants.DeleteUser) &&
            !User.IsDeleted &&
            User.PowerLevel > targetUser.PowerLevel;
    }

    public bool CanRestoreUser(User targetUser)
    {
        return User.Id != targetUser.Id &&
            User.IsDeleted &&
            User.HasPermission(PermissionNameConstants.RestoreUser);
    }

    public bool CanAssignToRole(Role role)
    {
        List<string> roleNames = User.Roles.Select(r => r.Name).ToList();
        return roleNames.Contains(RoleNameConstants.Developer) ||
            roleNames.Contains(RoleNameConstants.Manager) ||
            User.PowerLevel > role.PowerLevel;
    }

    public bool CanAssignRole()
    {
        return User.HasPermission(PermissionNameConstants.AssignRole);
    }

    public bool CanGetNote(int powerLevel)
    {
        return User.HasPermission(PermissionNameConstants.GetOtherUserNote) && User.PowerLevel > powerLevel;
    }

    // Permissions to interact with other resources.
    public bool CanCreate<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>
    {
        return User.HasPermission($"Create{typeof(TEntity).Name}");
    }

    public bool CanEdit<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>
    {
        return User.HasPermission($"Edit{typeof(TEntity).Name}");
    }

    public bool CanEdit<TEntity, TData>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class
    {
        return User.HasPermission($"Edit{typeof(TEntity).Name}") && !entity.IsLocked() && !entity.IsDeleted;
    }

    public bool CanDelete<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>
    {
        return User.HasPermission($"Delete{typeof(TEntity).Name}");
    }

    public bool CanDelete<TEntity, TData>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class
    {
        return User.HasPermission($"Edit{typeof(TEntity).Name}") && !entity.IsLocked() && !entity.IsDeleted;
    }

    public bool CanSetStatsDateTimeWhenCreating<TEntity, TData>()
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class
    {
        return User.HasPermission($"Set{typeof(TEntity).Name}StatsDateTime");
    }

    public bool CanSetStatsDateTimeWhenEditing<TEntity, TData>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class
    {
        return User.HasPermission($"Set{typeof(TEntity).Name}StatsDateTime") && !entity.IsLocked() && !entity.IsDeleted;
    }

    public bool CanAccessUpdateHistory<TEntity, TData>()
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class
    {
        return User.HasPermission($"Access{typeof(TEntity).Name}UpdateHistories");
    }
    #endregion
}
