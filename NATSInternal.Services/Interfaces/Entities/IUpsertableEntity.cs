namespace NATSInternal.Services.Interfaces.Entities;

internal interface IUpsertableEntity : IEntity
{
    DateTime CreatedDateTime { get; set; }
}