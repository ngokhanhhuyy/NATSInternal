namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle the <b>internal</b> operations which are related to authorization.
/// </summary>
internal interface IAuthorizationInternalService : IAuthorizationService
{
    #region Methods
    // Authorization for users.
    UserBasicAuthorizationResponseDto GetUserBasicAuthorization(User targetUser);
    UserDetailAuthorizationResponseDto GetUserDetailAuthorization(User targetUser);

    // Authorization for other resources.
    TResponseDto GetCreatingAuthorization<TEntity, TData, TResponseDto>()
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class
            where TResponseDto : IHasStatsCreatingAuthorizationResponseDto, new();

    TResponseDto GetExistingAuthorization<TEntity, TResponseDto>()
            where TEntity : class, IUpsertableEntity<TEntity>
            where TResponseDto : IUpsertableExistingAuthorizationResponseDto, new();

    TResponseDto GetExistingAuthorization<TEntity, TData, TResponseDto>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class
            where TResponseDto : IHasStatsExistingAuthorizationResponseDto, new();

    // Permissions to interact with users.
    bool CanCreateUser();
    bool CanEditUserPersonalInformation(User targetUser);
    bool CanEditUserUserInformation(User targetUser);
    bool CanChangeUserPassword(User targetUser);
    bool CanResetUserPassword(User targetUser);
    bool CanDeleteUser(User targetUser);
    bool CanRestoreUser(User targetUser);
    bool CanAssignToRole(Role role);

    // Permissions to interact with other resources.
    bool CanCreate<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>;

    bool CanEdit<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>;

    bool CanEdit<TEntity, TData>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class;

    bool CanDelete<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>;

    bool CanDelete<TEntity, TData>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class;

    bool CanSetStatsDateTimeWhenCreating<TEntity, TData>()
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class;

    bool CanSetStatsDateTimeWhenEditing<TEntity, TData>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class;
            
    bool CanAccessUpdateHistory<TEntity, TData>()
            where TEntity : class, IHasStatsEntity<TEntity, TData>
            where TData : class;
    #endregion
}