using NATSInternal.Domain.Features.Users;
using System.Text.Json;

namespace NATSInternal.Infrastructure.PersistenceModels;

internal class AuditLog
{
    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public required Guid TargetResourceId { get; set; }
    public required string ActionName { get; set; }
    public string? SnapshotJson { get; set; }
    public required DateTime LoggedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    public required Guid PerformedUserId { get; set; }
    #endregion

    #region NavigationProperties
    public User PerformedUser { get; set; } = null!;
    #endregion
    
    #region Methods
    public TSnapshot? GetSnapshot<TSnapshot>() where TSnapshot : class
    {
        if (SnapshotJson is null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<TSnapshot>(SnapshotJson);
    }

    public void SetSnapshot<TSnapshot>(TSnapshot snapshot) where TSnapshot : class
    {
        SnapshotJson = JsonSerializer.Serialize(snapshot);
    }
    #endregion
}