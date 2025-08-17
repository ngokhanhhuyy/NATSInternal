namespace NATSInternal.Core.Interfaces.Entities;

internal interface ICreatorTrackableEntity<T> : IUpsertableEntity<T> where T : class
{
    #region Properties
    Guid CreatedUserId { get; set; }
    User CreatedUser { get; set; }
    #endregion
}