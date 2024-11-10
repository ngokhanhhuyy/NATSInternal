using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace NATSInternal.Services;

internal class AuthorizationService : IAuthorizationInternalService
{
    private readonly DatabaseContext _context;
    private User _user;

    public AuthorizationService(
            DatabaseContext context,
            IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        ClaimsPrincipal user = httpContextAccessor.HttpContext?.User;
        
        string userIdAsString = user?.FindFirstValue(ClaimTypes.NameIdentifier);
        bool parsable = int.TryParse(userIdAsString, out int userId);
        if (!parsable)
        {
            throw new AuthenticationException();
        }
        
        SetUserId(userId);
    }

    public void SetUserId(int id)
    {
        _user = _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .Single(u => u.Id == id);
    }
    
    public int GetUserId()
    {
        return _user.Id;
    }

    public UserDetailResponseDto GetUserDetail()
    {
        return new UserDetailResponseDto(_user);
    }

    // Authorization for users.
    public UserBasicAuthorizationResponseDto GetUserBasicAuthorization(User targetUser)
    {
        return new UserBasicAuthorizationResponseDto
        {
            CanEdit = CanEditUserPersonalInformation(targetUser) ||
                CanEditUserUserInformation(targetUser),
            CanChangePassword = CanChangeUserPassword(targetUser),
            CanResetPassword = CanResetUserPassword(targetUser),
            CanDelete = CanDeleteUser(targetUser)
        };
    }

    public UserDetailAuthorizationResponseDto GetUserDetailAuthorization(User targetUser)
    {
        UserDetailAuthorizationResponseDto dto = new UserDetailAuthorizationResponseDto
        {
            CanGetNote = CanGetNote(targetUser.PowerLevel),
            CanEdit = CanEditUserPersonalInformation(targetUser) ||
                CanEditUserUserInformation(targetUser),
            CanEditUserPersonalInformation = CanEditUserPersonalInformation(targetUser),
            CanEditUserUserInformation = CanEditUserUserInformation(targetUser),
            CanAssignRole = CanAssignRole(),
            CanChangePassword = CanChangeUserPassword(targetUser),
            CanResetPassword = CanResetUserPassword(targetUser),
            CanDelete = CanDeleteUser(targetUser)
        };
        return new UserDetailAuthorizationResponseDto
        {
            CanGetNote = CanGetNote(targetUser.PowerLevel),
            CanEdit = CanEditUserPersonalInformation(targetUser) ||
                CanEditUserUserInformation(targetUser),
            CanEditUserPersonalInformation = CanEditUserPersonalInformation(targetUser),
            CanEditUserUserInformation = CanEditUserUserInformation(targetUser),
            CanAssignRole = CanAssignRole(),
            CanChangePassword = CanChangeUserPassword(targetUser),
            CanResetPassword = CanResetUserPassword(targetUser),
            CanDelete = CanDeleteUser(targetUser)
        };
    }

    // Authorization for other resources.
    public TResponseDto GetCreatingAuthorization<TEntity, TUpdateHistoryEntity, TResponseDto>()
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new()
            where TResponseDto : IHasStatsCreatingAuthorizationResponseDto, new()
    {
        return new TResponseDto
        {
            CanSetStatsDateTime = CanSetStatsDateTimeWhenCreating<
                TEntity,
                TUpdateHistoryEntity>()
        };
    }

    public TResponseDto GetExistingAuthorization<TEntity, TResponseDto>()
            where TEntity : class, IUpsertableEntity<TEntity>, new()
            where TResponseDto : IUpsertableExistingAuthorizationResponseDto, new()
    {
        return new TResponseDto
        {
            CanEdit = CanEdit<TEntity>(),
            CanDelete = CanDelete<TEntity>()
        };
    }

    public TResponseDto GetExistingAuthorization<TEntity, TUpdateHistoryEntity, TResponseDto>(
            TEntity entity)
                where TEntity :
                    class,
                    IHasStatsEntity<TEntity, TUpdateHistoryEntity>,
                    new()
                where TUpdateHistoryEntity :
                    class,
                    IUpdateHistoryEntity<TUpdateHistoryEntity>,
                    new()
                where TResponseDto : IHasStatsExistingAuthorizationResponseDto, new()
    {
        return new TResponseDto
        {
            CanEdit = CanEdit<TEntity, TUpdateHistoryEntity>(entity),
            CanDelete = CanDelete<TEntity, TUpdateHistoryEntity>(entity),
            CanSetStatsDateTime = CanSetStatsDateTimeWhenEditing<
                TEntity,
                TUpdateHistoryEntity>(entity)
        };
    }

    // Permissions to interact with users.
    public bool CanCreateUser()
    {
        return _user.HasPermission(PermissionConstants.CreateUser);
    }
    
    public bool CanEditUserPersonalInformation(User targetUser)
    {
        // Check permission when the user is editing himself.
        if (_user.Id == targetUser.Id &&
            _user.HasPermission(PermissionConstants.EditSelfPersonalInformation))
        {
            return true;
        }

        // Check permission when the user is editing another user.
        else if (_user.HasPermission(PermissionConstants.EditOtherUserPersonalInformation) &&
                _user.PowerLevel > targetUser.PowerLevel)
        {
            return true;
        }

        return false;
    }

    public bool CanEditUserUserInformation(User targetUser)
    {
        // Check permission when the user is editing himself.
        if (_user.Id == targetUser.Id &&
            _user.HasPermission(PermissionConstants.EditSelfUserInformation))
        {
            return true;
        }

        // Check permission when the user is editing another user.
        else if (_user.HasPermission(PermissionConstants.EditOtherUserUserInformation) &&
                _user.PowerLevel > targetUser.PowerLevel)
        {
            return true;
        }

        return false;
    }

    public bool CanChangeUserPassword(User targetUser)
    {
        return _user.Id == targetUser.Id;
    }

    public bool CanResetUserPassword(User targetUser)
    {
        return _user.Id != targetUser.Id &&
            _user.HasPermission(PermissionConstants.ResetOtherUserPassword) &&
            _user.PowerLevel > targetUser.PowerLevel;
    }

    public bool CanDeleteUser(User targetUser)
    {
        return _user.Id != targetUser.Id &&
            _user.HasPermission(PermissionConstants.DeleteUser) &&
            !_user.IsDeleted &&
            _user.PowerLevel > targetUser.PowerLevel;
    }

    public bool CanRestoreUser(User targetUser)
    {
        return _user.Id != targetUser.Id &&
                _user.IsDeleted &&
                _user.HasPermission(PermissionConstants.RestoreUser);
    }

    public bool CanAssignToRole(Role role)
    {
        return _user.Role.Name == RoleConstants.Developer ||
            _user.Role.Name == RoleConstants.Manager ||
            _user.PowerLevel > role.PowerLevel;
    }

    public bool CanAssignRole()
    {
        return _user.HasPermission(PermissionConstants.AssignRole);
    }

    public bool CanGetNote(int powerLevel)
    {
        return _user.HasPermission(PermissionConstants.GetOtherUserNote) &&
            _user.PowerLevel > powerLevel;
    }

    // Permissions to interact with other resources.
    public bool CanCreate<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>, new()
    {
        return _user.HasPermission($"Create{typeof(TEntity).Name}");
    }

    public bool CanEdit<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>, new()
    {
        return _user.HasPermission($"Edit{typeof(TEntity).Name}");
    }

    public bool CanEdit<TEntity, TUpdateHistoryEntity>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new()
    {
        return _user.HasPermission($"Edit{typeof(TEntity).Name}") &&
            !entity.IsLocked &&
            !entity.IsDeleted;
    }

    public bool CanDelete<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>, new()
    {
        return _user.HasPermission($"Delete{typeof(TEntity).Name}");
    }

    public bool CanDelete<TEntity, TUpdateHistoryEntity>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new()
    {
        return _user.HasPermission($"Edit{typeof(TEntity).Name}") &&
            !entity.IsLocked &&
            !entity.IsDeleted;
    }

    public bool CanSetStatsDateTimeWhenCreating<TEntity, TUpdateHistoryEntity>()
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new()
    {
        return _user.HasPermission($"Set{typeof(TEntity).Name}StatsDateTime");
    }

    public bool CanSetStatsDateTimeWhenEditing<TEntity, TUpdateHistoryEntity>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new()
    {
        return _user.HasPermission($"Set{typeof(TEntity).Name}StatsDateTime") &&
            !entity.IsLocked &&
            !entity.IsDeleted;
    }

    public bool CanAccessUpdateHistory<TEntity, TUpdateHistoryEntity>()
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new()
    {
        return _user.HasPermission($"Access{typeof(TEntity).Name}UpdateHistories");
    }
}
