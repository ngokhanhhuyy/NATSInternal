namespace NATSInternal.Application.AuditLogs;

public class AuditLogResponseDto<TSnapshot> where TSnapshot : class
{
    #region Constructors
    public AuditLogResponseDto(Guid id, TSnapshot snapshot, string actionName)
    {
        Id = id;
        Snapshot = snapshot;
        ActionName = actionName;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public TSnapshot Snapshot { get; }
    public string ActionName { get; }
    #endregion
}