namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICreatorTrackableEntity<T> : IUpsertableEntity<T>
    where T : class, new()
{
    int CreatedUserId { get; set; }
    User CreatedUser { get; set; }
}