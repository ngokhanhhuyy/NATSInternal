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

    private AuditLog(
        Guid targetResourceId,
        string actionType,
        string? dataJsonBeforeModification,
        string? dataJsonAfterModification,
        DateTime loggedDateTime)
    {
        TargetResourceId = targetResourceId;
        ActionType = actionType;
        LoggedDateTime = loggedDateTime;
        _dataJsonBeforeModification = dataJsonBeforeModification;
        _dataJsonAfterModification = dataJsonAfterModification;
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TargetResourceId { get; private set; }
    public string ActionType { get; private set; }
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
    #endregion

    #region StaticMethods
    public static AuditLog NewUserCreatingAuditLog<TData>(
        Guid targetUserId,
        TData userData,
        DateTime loggedDateTime) where TData : class
    {
        string userDataJson = JsonSerializer.Serialize(userData);
        return new(targetUserId, AuditLogActionType.UserCreate, null, userDataJson, loggedDateTime);
    }
    #endregion
}