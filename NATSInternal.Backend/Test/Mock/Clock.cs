using NATSInternal.Core.Common.Time;

namespace NATSInternal.Test.Mock;

public class Clock : IClock
{
    #region Fields
    private DateTime _currentDateTime = new(2026, 5, 10, 12, 0, 0);
    #endregion

    #region Properties
    public DateTime Now => _currentDateTime;
    public DateOnly Today => DateOnly.FromDateTime(_currentDateTime);
    #endregion

    #region Methods
    public void SetCurrentDateTime(DateTime currentDateTime)
    {
        _currentDateTime = currentDateTime;
    }
    #endregion
}