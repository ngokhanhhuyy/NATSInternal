namespace NATSInternal.Core.Interfaces.Services;

public interface IStatsTaskService
{
    DateTime StartedDateTime { get; }
    DateTime ExpectedRestartingDateTime { get; }
    TimeSpan RunningTime { get; }
    TimeSpan RemainingTime { get; }
}
