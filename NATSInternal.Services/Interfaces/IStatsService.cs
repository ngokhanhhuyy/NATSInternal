namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle the operations which are related to statistics.
/// </summary>
public interface IStatsService
{
    /// <summary>
    /// Get daily stats by a given recorded date. If the date is not
    /// specified, the returned stats will be today stats.
    /// </summary>
    /// <param name="recordedDate">
    /// The date when the daily stats was recorded.
    /// </param>
    /// <returns>
    /// An object containing the detail information of the daily stats.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the stats recorded at the specified date doesn't exist.
    /// </exception>
    Task<DailyStatsDetailResponseDto> GetDailyStatsDetailAsync(DateOnly? recordedDate);

    /// <summary>
    /// Get monthly stats by a given recorded month and year. If the date
    /// is not specified, the returned stats will be this month stats.
    /// </summary>
    /// <param name="requestDto">
    /// An object containing expected recorded year and month when the monthly stats
    /// was recorded.
    /// </param>
    /// <returns>
    /// An object containing the detail information of the monthly stats.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the stats recorded at the specified month and year
    /// doesn't exist.
    /// </exception>
    Task<MonthlyStatsDetailResponseDto> GetMonthlyStatsDetailAsync(
            MonthlyStatsRequestDto requestDto);
}
