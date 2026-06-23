using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Common.Services;

public interface ITopAndCountService
{
    #region Methods
    Task<TopOverTimeRangeResponseDto<TBasicResponseDto, TMetric>> GetTopAsync<TBasicResponseDto, TMetric>(
        TopRequestDto requestDto,
        Func<DateOnly, IQueryable<TopItemResponseDto<TBasicResponseDto, TMetric>>> getQuery)
            where TBasicResponseDto : class;
            
    Task<CountOverTimeRangeResponseDto<int>> GetCountAsync(
        CountOverTimeRangeRequestDto requestDto,
        Func<DateOnly, DateOnly, Task<int>> getTask);
            
    Task<CountOverTimeRangeResponseDto<long>> GetCountAsync(
        CountOverTimeRangeRequestDto requestDto,
        Func<DateOnly, DateOnly, Task<long>> getTask);
    #endregion
}
