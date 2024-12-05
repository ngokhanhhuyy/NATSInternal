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

    /// <summary>
    /// Retrieves the top of the sold products with basic information, based on the creteria
    /// and count in a lastest time-range specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO instance containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO
    /// containing the results and range information.
    /// </returns>
    Task<TopSoldProductListResponseDto> GetTopSoldProductListAsync(
            TopSoldProductListRequestDto requestDto);

    /// <summary>
    /// Retrieves the range type options which can be used as conditions for top sold product
    /// retrieving operation.
    /// </summary>
    /// <returns>
    /// A DTO containing a <see cref="List{T}"/> of options with display names and the default
    /// option if the request DTO for the operation doesn't include one.
    /// </returns>
    StatsRangeTypeOptionListResponseDto GetTopSoldProductRangeTypeOptions();

    /// <summary>
    /// Retrieves the criteria options which can be used as conditions for top sold product
    /// retrieving operation.
    /// </summary>
    /// <returns>
    /// A DTO containing a <see cref="List{T}"/> of options with display names and the default
    /// option if the request DTO for the operation doesn't include one.
    /// </returns>
    StatsCriteriaOptionListResponseDto GetTopSoldProductCriteriaOptions();

    /// <summary>
    /// Retrieves the top of the purchased customers with basic information, based on the
    /// creteria and count in a lastest time-range specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO instance containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO
    /// containing the results and range information.
    /// </returns>
    Task<TopPurchasedCustomerListResponseDto> GetTopPurchasedCustomerListAsync(
            TopPurchasedCustomerListRequestDto requestDto);

    /// <summary>
    /// Retrieves the range type options which can be used as conditions for top purchased
    /// customer retrieving operation.
    /// </summary>
    /// <returns>
    /// A DTO containing a <see cref="List{T}"/> of options with display names and the default
    /// option if the request DTO for the operation doesn't include one.
    /// </returns>
    StatsRangeTypeOptionListResponseDto GetTopPuschasedCustomerRangeTypeOptions();

    /// <summary>
    /// Retrieves the criteria options which can be used as conditions for top purchased
    /// customer retrieving operation.
    /// </summary>
    /// <returns>
    /// A DTO containing a <see cref="List{T}"/> of options with display names and the default
    /// option if the request DTO for the operation doesn't include one.
    /// </returns>
    StatsCriteriaOptionListResponseDto GetTopPurchasedCustomerCriteriaOptions();

    /// <summary>
    /// Retrieves the lastest transactions, based on the transaction count specified in the
    /// request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the results.
    /// </returns>
    Task<List<LastestTransactionResponseDto>> GetLastestTransactionsAsync(
            LastestTransactionsRequestDto requestDto);
}
