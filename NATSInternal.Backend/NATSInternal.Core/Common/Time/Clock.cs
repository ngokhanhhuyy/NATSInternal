namespace NATSInternal.Core.Common.Time;

internal class Clock : IClock
{
    #region Properties
    public DateTime Now => DateTime.UtcNow.AddHours(7);
    public DateOnly Today => DateOnly.FromDateTime(Now);
    #endregion
}