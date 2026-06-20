using JetBrains.Annotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Enums;
using NATSInternal.Core.Common.Time;
using System.Numerics;

namespace NATSInternal.Core.Common.Services;

[UsedImplicitly]
internal class TopAndCountService : ITopAndCountService
{
    #region Fields
    private readonly IValidator<TopRequestDto> _topValidator;
    private readonly IValidator<CountRequestDto> _countValidator;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public TopAndCountService(
        IValidator<TopRequestDto> topValidator,
        IValidator<CountRequestDto> countValidator, IClock clock)
    {
        _topValidator = topValidator;
        _countValidator = countValidator;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task<TopResponseDto<TBasicResponseDto, TMetric>> GetTopAsync<TBasicResponseDto, TMetric>(
        TopRequestDto requestDto,
        Func<DateOnly, IQueryable<TopItemResponseDto<TBasicResponseDto, TMetric>>> getQuery)
            where TBasicResponseDto : class
    {
        _topValidator.ValidateAndThrow(requestDto);

        TimeRangeDates date = GetTimeRangeDates(requestDto);
        DateOnly earliestDate = date.CurrentTimeRangeMinDate;
        List<TopItemResponseDto<TBasicResponseDto, TMetric>> itemResponseDtos = await getQuery(earliestDate)
            .Take(requestDto.ResultsCount)
            .ToListAsync();

        return new(itemResponseDtos);
    }

    public async Task<CountResponseDto<int>> GetCountAsync(
        CountRequestDto requestDto,
        Func<DateOnly, DateOnly, Task<int>> getTask)
    {
        TimeRangeCounts<int> counts = await GetCountsAsync(requestDto, getTask);
        int percentage = (int)Math.Ceiling((double)counts.CurrentTimeRangeCount / counts.PreviousTimeRangeCount * 100);
        int percentageDifference = percentage - 100;

        return new(counts.CurrentTimeRangeCount, counts.PreviousTimeRangeCount, percentageDifference);
    }

    public async Task<CountResponseDto<long>> GetCountAsync(
        CountRequestDto requestDto,
        Func<DateOnly, DateOnly, Task<long>> getTask)
    {
        TimeRangeCounts<long> counts = await GetCountsAsync(requestDto, getTask);
        int percentage = (int)Math.Ceiling((double)counts.CurrentTimeRangeCount / counts.PreviousTimeRangeCount * 100);
        int percentageDifference = percentage - 100;

        return new(counts.CurrentTimeRangeCount, counts.PreviousTimeRangeCount, percentageDifference);
    }
    #endregion

    #region PrivateMethods
    private async Task<TimeRangeCounts<TMetric>> GetCountsAsync<TMetric>(
        CountRequestDto requestDto,
        Func<DateOnly, DateOnly, Task<TMetric>> getTask) where TMetric : INumber<TMetric>
    {
        _countValidator.ValidateAndThrow(requestDto);

        TimeRangeDates dates = GetTimeRangeDates(requestDto);

        return new()
        {
            CurrentTimeRangeCount = await getTask(
                dates.CurrentTimeRangeMinDate,
                dates.CurrentTimeRangeMaxDate),
            PreviousTimeRangeCount = await getTask(
                dates.PreviousTimeRangeMinDate,
                dates.PreviousTimeRangeMaxDate)
        };
    }

    private TimeRangeDates GetTimeRangeDates(ITopAndCountRequestDto requestDto)
    {
        DateOnly GetMinDate(DateOnly baseDate)
        {
            switch (requestDto.TimeRangeUnitType)
            {
                case TimeRangeUnitType.Year:
                    return baseDate.AddYears(-requestDto.TimeRangeUnitCount);
                case TimeRangeUnitType.Month:
                    return baseDate.AddMonths(-requestDto.TimeRangeUnitCount);
                default:
                case TimeRangeUnitType.Day:
                    return baseDate.AddDays(-requestDto.TimeRangeUnitCount);
            }
        }

        DateOnly today = _clock.Today;
        DateOnly currentTimeRangeMinDate = GetMinDate(today);
        DateOnly previousTimeRangeMinDate = GetMinDate(currentTimeRangeMinDate);

        return new()
        {
            CurrentTimeRangeMinDate = currentTimeRangeMinDate,
            CurrentTimeRangeMaxDate = today,
            PreviousTimeRangeMinDate = previousTimeRangeMinDate,
            PreviousTimeRangeMaxDate = currentTimeRangeMinDate
        };
    }
    #endregion

    #region Classes
    record TimeRangeDates
    {
        #region Properties
        public required DateOnly CurrentTimeRangeMinDate { get; set; }
        public required DateOnly CurrentTimeRangeMaxDate { get; set; }
        public required DateOnly PreviousTimeRangeMinDate { get; set; }
        public required DateOnly PreviousTimeRangeMaxDate { get; set; }
        #endregion
    }

    record TimeRangeCounts<TMetric> where TMetric : INumber<TMetric>
    {
        #region Properties
        public required TMetric CurrentTimeRangeCount { get; set; }
        public required TMetric PreviousTimeRangeCount { get; set; }
        #endregion
    }
    #endregion
}
