namespace NATSInternal.Core.Common.Time;

public interface IClock
{
    #region Properties
    public DateTime Now { get; }
    public DateOnly Today { get; }
    #endregion
}