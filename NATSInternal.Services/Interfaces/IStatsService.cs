namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle the operations which are related to statistics.
/// </summary>
public interface IStatsService
{
    Task<MonthlyStatsBasicResponseDto> GetMonthlyBasicAsync(
        MonthlyStatsRequestDto requestDto);
    
    /// <summary>
    /// Retrieves daily stats with details by a given recorded date. If the date is not
    /// specified, the returned stats will be today stats.
    /// </summary>
    /// <param name="recordedDate">
    /// The date when the daily stats was recorded.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchonous operation, which result is a DTO
    /// containing the detail information of the daily stats.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the stats recorded at the specified date doesn't exist.
    /// </exception>
    Task<DailyStatsDetailResponseDto> GetDailyDetailAsync(DateOnly? recordedDate);

    /// <summary>
    /// Retrieves the stats of lastest dates with basic information, specified by the count of
    /// dates, from today (if the <paramref name="requestDto"/> specifies) or yesterday to the
    /// past.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO indicating the count number of dates and whether today's data should be included.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchonous operation, which result is a DTO
    /// containing the basic information of the dates' stats.
    /// </returns>
    Task<List<DailyStatsBasicResponseDto>> GetLastestDailyBasicAsync(
            LastestDailyStatsRequestDto requestDto);

    /// <summary>
    /// Retrieves the stats of lastest dates with details, specified by the count of dates,
    /// from today (if the <paramref name="requestDto"/> specifies) or yesterday to the past.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO indicating the count number of dates and whether today's data should be included.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchonous operation, which result is a DTO
    /// containing the basic information of the dates' stats.
    /// </returns>
    Task<List<DailyStatsDetailResponseDto>> GetLastestDailyDetailAsync(
            LastestDailyStatsRequestDto requestDto);

    /// <summary>
    /// Retrieves monthly stats with details by a given recorded month and year. If the date is
    /// not specified, the returned stats will be this month stats.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO instance containing expected recorded year and month when the monthly stats was
    /// recorded.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> representing the asynchronous operation, which result is a DTO
    /// containing the detail information of the monthly stats.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Thrown when the stats recorded at the specified month and year doesn't exist.
    /// </exception>
    Task<MonthlyStatsDetailResponseDto> GetMonthlyDetailAsync(
            MonthlyStatsRequestDto requestDto);

    /// <summary>
    /// Retrieves the stats of lastest months with basic information, specified by the count
    /// of months, from this month (if the <paramref name="requestDto"/> specifies), to the
    /// past.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO instance indicating the count number of months and whethere the this month data
    /// should be included.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchonous operation, which result is a DTO
    /// containing the basic information of the months' stats.
    /// </returns>
    Task<List<MonthlyStatsBasicResponseDto>> GetLastestMonthlyAsync(
            LastestMonthlyStatsRequestDto requestDto);
}
