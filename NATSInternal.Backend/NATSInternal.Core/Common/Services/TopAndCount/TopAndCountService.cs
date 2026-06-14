using JetBrains.Annotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Enums;
using NATSInternal.Core.Common.Time;

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
    public TopAndCountService(IValidator<TopRequestDto> topValidator, IValidator<CountRequestDto> countValidator, IClock clock)
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

        DateOnly earliestDate = GetEarliestDateFromTimeRange(requestDto);
        List<TopItemResponseDto<TBasicResponseDto, TMetric>> itemResponseDtos = await getQuery(earliestDate)
            .Take(requestDto.ResultsCount)
            .ToListAsync();

        return new(itemResponseDtos);
    }

    public async Task<int> GetCountAsync(CountRequestDto requestDto, Func<DateOnly, IQueryable<int>> getQuery)
    {
        _countValidator.ValidateAndThrow(requestDto);

        DateOnly earliestDate = GetEarliestDateFromTimeRange(requestDto);
        IQueryable<int> query = getQuery(earliestDate);
        return await query.SingleAsync();
    }
    #endregion

    #region PrivateMethods
    private DateOnly GetEarliestDateFromTimeRange(ITopAndCountRequestDto requestDto)
    {
        DateOnly today = _clock.Today;
        switch (requestDto.TimeRangeUnitType)
        {
            case TopTimeRangeUnitType.Year:
                return today.AddYears(-requestDto.TimeRangeUnitCount);
            case TopTimeRangeUnitType.Month:
                return today.AddMonths(-requestDto.TimeRangeUnitCount);
            default:
            case TopTimeRangeUnitType.Day:
                return  today.AddDays(-requestDto.TimeRangeUnitCount);
        }
    }
    #endregion
}
