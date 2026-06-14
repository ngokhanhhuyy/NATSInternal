using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Common.Services;

public interface ITopAndCountService
{
    #region Methods
    Task<int> GetCountAsync(CountRequestDto requestDto, Func<DateOnly, IQueryable<int>> getQuery);

    Task<TopResponseDto<TBasicResponseDto, TMetric>> GetTopAsync<TBasicResponseDto, TMetric>(
        TopRequestDto requestDto,
        Func<DateOnly, IQueryable<TopItemResponseDto<TBasicResponseDto, TMetric>>> getQuery)
            where TBasicResponseDto : class;
    #endregion
}
