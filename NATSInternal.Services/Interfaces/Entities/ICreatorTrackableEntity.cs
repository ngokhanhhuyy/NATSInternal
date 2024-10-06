namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICreatorTrackableEntity<T, TUser> : IUpsertableEntity<T>
    where T : class, new()
    where TUser : class, IUserEntity<TUser>, new()
{
    int CreatedUserId { get; set; }
    TUser CreatedUser { get; set; }
}