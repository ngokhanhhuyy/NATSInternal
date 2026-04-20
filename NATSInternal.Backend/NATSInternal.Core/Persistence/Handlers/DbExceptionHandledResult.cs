namespace NATSInternal.Core.Persistence.Handlers;

internal class DbExceptionHandledResult
{
    #region Properties
    public bool IsUniqueConstraintViolation { get; set; }
    public bool IsForeignKeyConstraintViolation { get; set; }
    public bool IsNotNullConstraintViolation { get; set; }
    public bool IsConcurrencyConflict { get; set; }
    public string? ViolatedPropertyName { get; set; }
    public string? ViolatedEntityName { get; set; }
    #endregion
}