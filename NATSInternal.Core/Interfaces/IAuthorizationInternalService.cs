namespace NATSInternal.Core.Interfaces;

/// <summary>
/// A service to handle the <b>internal</b> operations which are related to authorization.
/// </summary>
internal interface IAuthorizationInternalService : IAuthorizationService
{
    // Authorization for users.
    UserBasicAuthorizationResponseDto GetUserBasicAuthorization(User targetUser);
    UserDetailAuthorizationResponseDto GetUserDetailAuthorization(User targetUser);

    // Authorization for other resources.
    TResponseDto GetCreatingAuthorization<TEntity, TUpdateHistoryEntity, TResponseDto>()
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new()
            where TResponseDto : IHasStatsCreatingAuthorizationResponseDto, new();

    TResponseDto GetExistingAuthorization<TEntity, TResponseDto>()
            where TEntity : class, IUpsertableEntity<TEntity>, new()
            where TResponseDto : IUpsertableExistingAuthorizationResponseDto, new();

    TResponseDto GetExistingAuthorization<TEntity, TUpdateHistoryEntity, TResponseDto>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new()
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
    bool CanCreate<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>, new();
    bool CanEdit<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>, new();
    bool CanEdit<TEntity, TUpdateHistoryEntity>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new();
    bool CanDelete<TEntity>() where TEntity : class, IUpsertableEntity<TEntity>, new();
    bool CanDelete<TEntity, TUpdateHistoryEntity>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new();
    bool CanSetStatsDateTimeWhenCreating<TEntity, TUpdateHistoryEntity>()
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new();
    bool CanSetStatsDateTimeWhenEditing<TEntity, TUpdateHistoryEntity>(TEntity entity)
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new();
    bool CanAccessUpdateHistory<TEntity, TUpdateHistoryEntity>()
            where TEntity : class, IHasStatsEntity<TEntity, TUpdateHistoryEntity>, new()
            where TUpdateHistoryEntity : class, IUpdateHistoryEntity<TUpdateHistoryEntity>, new();
}