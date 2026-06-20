using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Common.Services;

public interface ITopAndCountService
{
    #region Methods
    Task<TopResponseDto<TBasicResponseDto, TMetric>> GetTopAsync<TBasicResponseDto, TMetric>(
        TopRequestDto requestDto,
        Func<DateOnly, IQueryable<TopItemResponseDto<TBasicResponseDto, TMetric>>> getQuery)
            where TBasicResponseDto : class;
            
    Task<CountResponseDto<int>> GetCountAsync(
        CountRequestDto requestDto,
        Func<DateOnly, DateOnly, Task<int>> getTask);
            
    Task<CountResponseDto<long>> GetCountAsync(
        CountRequestDto requestDto,
        Func<DateOnly, DateOnly, Task<long>> getTask);
    #endregion
}
