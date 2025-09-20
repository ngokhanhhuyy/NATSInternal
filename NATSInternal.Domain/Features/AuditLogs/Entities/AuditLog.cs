using System.Text.Json;

namespace NATSInternal.Domain.Features.AuditLogs;

internal class AuditLog
{
    #region Fields
    private readonly string? _dataJsonBeforeModification;
    private readonly string? _dataJsonAfterModification;
    #endregion
    
    #region Constructors
    #nullable disable
    private AuditLog() { }
    #nullable enable

    public AuditLog(
        Guid targetResourceId,
        Guid performedUserId,
        string actionName,
        string? dataJsonBeforeModification,
        string? dataJsonAfterModification,
        DateTime loggedDateTime)
    {
        TargetResourceId = targetResourceId;
        PerformedUserId = performedUserId;
        ActionName = actionName;
        LoggedDateTime = loggedDateTime;
        _dataJsonBeforeModification = dataJsonBeforeModification;
        _dataJsonAfterModification = dataJsonAfterModification;
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TargetResourceId { get; private set; }
    public Guid PerformedUserId { get; private set; }
    public string ActionName { get; private set; }
    public DateTime LoggedDateTime { get; private set; }
    #endregion
    
    #region Methods
    public TData? GetDataBeforeModification<TData>() where TData : class
    {
        if (_dataJsonBeforeModification is null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<TData>(_dataJsonBeforeModification);
    }

    public TData? GetDataAfterModification<TData>() where TData : class
    {
        if (_dataJsonAfterModification is null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<TData>(_dataJsonAfterModification);
    }
    #endregion
}