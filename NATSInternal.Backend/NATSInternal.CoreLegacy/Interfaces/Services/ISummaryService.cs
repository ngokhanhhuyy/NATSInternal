namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle the operations which are related to summaries.
/// </summary>
public interface ISummaryService
{
    Task<MonthlySummaryBasicResponseDto> GetMonthlyBasicAsync(
            MonthlyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves daily summary with details by a given recorded date. If the date is not specified, the returned
    /// summary will be today summary.
    /// </summary>
    /// <param name="recordedDate">
    /// The date when the daily summary was recorded.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchonous operation, which result is a DTO containing the detail
    /// information of the daily summary.
    /// </returns>
    Task<DailySummaryDetailResponseDto> GetDailyDetailAsync(
            DateOnly? recordedDate,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the summary of latest dates with basic information, specified by the count of dates, from today (if
    /// the <paramref name="requestDto"/> specifies) or yesterday to the past.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO indicating the count number of dates and whether today's data should be included.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchonous operation, which result is a DTO containing the basic
    /// information of the dates' summary.
    /// </returns>
    Task<List<DailySummaryBasicResponseDto>> GetLastestDailyBasicAsync(
            LatestDailyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the summary of latest dates with details, specified by the count of dates, from today (if the
    /// <paramref name="requestDto"/> specifies) or yesterday to the past.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO indicating the count number of dates and whether today's data should be included.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchonous operation, which result is a DTO containing the basic
    /// information of the dates' summary.
    /// </returns>
    Task<List<DailySummaryDetailResponseDto>> GetLastestDailyDetailAsync(
            LatestDailyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves monthly summary with details by a given recorded month and year. If the date is not specified, the
    /// returned summary will be this month summary.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO instance containing expected recorded year and month when the monthly summary was recorded.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task" /> representing the asynchronous operation, which result is a DTO containing the detail
    /// information of the monthly summary.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when the summary recorded at the specified month and year doesn't exist.
    /// </exception>
    Task<MonthlyStatsDetailResponseDto> GetMonthlyDetailAsync(
            MonthlyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the summary of latest months with basic information, specified by the count of months, from this
    /// month (if the <paramref name="requestDto"/> specifies), to the past.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO instance indicating the count number of months and whethere the this month data should be included.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchonous operation, which result is a DTO containing the basic
    /// information of the months' summary.
    /// </returns>
    Task<List<MonthlySummaryBasicResponseDto>> GetLastestMonthlyAsync(
            LatestMonthlyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the top of the sold products with basic information, based on the creteria and count in a latest
    /// time-range specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO instance containing the conditions for the results.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO containing the results and
    /// range information.
    /// </returns>
    Task<TopSoldProductListResponseDto> GetTopSoldProductListAsync(
            TopSoldProductListRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the range type options which can be used as conditions for top sold product retrieving operation.
    /// </summary>
    /// <returns>
    /// A DTO containing a <see cref="List{T}"/> of options with display names and the default option if the request DTO
    /// for the operation doesn't include one.
    /// </returns>
    StatsRangeTypeOptionListResponseDto GetTopSoldProductRangeTypeOptions();

    /// <summary>
    /// Retrieves the criteria options which can be used as conditions for top sold product retrieving operation.
    /// </summary>
    /// <returns>
    /// A DTO containing a <see cref="List{T}"/> of options with display names and the default option if the request DTO
    /// for the operation doesn't include one.
    /// </returns>
    StatsCriteriaOptionListResponseDto GetTopSoldProductCriteriaOptions();

    /// <summary>
    /// Retrieves the top of the purchased customers with basic information, based on the creteria and count in a latest
    /// time-range specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO instance containing the conditions for the results.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO containing the results and
    /// range information.
    /// </returns>
    Task<TopPurchasedCustomerListResponseDto> GetTopPurchasedCustomerListAsync(
            TopPurchasedCustomerListRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the range type options which can be used as conditions for top purchased customer retrieving
    /// operation.
    /// </summary>
    /// <returns>
    /// A DTO containing a <see cref="List{T}"/> of options with display names and the default option if the request DTO
    /// for the operation doesn't include one.
    /// </returns>
    StatsRangeTypeOptionListResponseDto GetTopPuschasedCustomerRangeTypeOptions();

    /// <summary>
    /// Retrieves the criteria options which can be used as conditions for top purchased customer retrieving operation.
    /// </summary>
    /// <returns>
    /// A DTO containing a <see cref="List{T}"/> of options with display names and the default option if the request DTO
    /// for the operation doesn't include one.
    /// </returns>
    StatsCriteriaOptionListResponseDto GetTopPurchasedCustomerCriteriaOptions();

    /// <summary>
    /// Retrieves the latest transactions, based on the transaction count specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the conditions for the results.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a <see cref="List{T}"/> of DTOs,
    /// containing the results.
    /// </returns>
    Task<List<LastestTransactionResponseDto>> GetLatestTransactionsAsync(
            LatestTransactionsRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the summary date as options.
    /// </summary>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a <see cref="List{T}"/> of
    /// <see cref="DateOnly"/>, representing the dates.
    /// </returns>
    Task<List<DateOnly>> GetSummaryDateOptionsAsync(CancellationToken cancellationToken = default);
}
