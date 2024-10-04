namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICreatorTrackableEntity
{
    int CreatedUserId { get; set; }
    IUserEntity CreatedUser { get; set; }
}