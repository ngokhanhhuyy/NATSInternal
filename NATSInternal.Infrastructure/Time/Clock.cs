using NATSInternal.Application.Time;

namespace NATSInternal.Infrastructure.Time;

public class Clock : IClock
{
    #region Properties
    public DateTime Now => DateTime.UtcNow.AddHours(7);

    public DateOnly Today => DateOnly.FromDateTime(Now);
    #endregion
}