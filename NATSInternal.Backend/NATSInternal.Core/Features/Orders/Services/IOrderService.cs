using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Orders;

public interface IOrderService
{
    #region Methods
    Task<OrderListResponseDto> GetListAsync(OrderListRequestDto requestDto);
    Task<OrderDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(OrderUpsertRequestDto requestDto);
    Task UpdateAsync(int id, OrderUpsertRequestDto requestDto);
    Task DeleteAsync(int id);
    Task<CountOverTimeRangeResponseDto<long>> GetRevenueAsync(CountOverTimeRangeRequestDto requestDto);
    Task<CountOverTimeRangeResponseDto<int>> GetCountAsync(CountOverTimeRangeRequestDto requestDto);
    Task<CountOverTimeRangeResponseDto<int>> GetConsultantCountAsync(CountOverTimeRangeRequestDto requestDto);
    Task<CountOverTimeRangeResponseDto<int>> GetRetailCountAsync(CountOverTimeRangeRequestDto requestDto);
    Task<CountOverTimeRangeResponseDto<int>> GetTreatmentCountAsync(CountOverTimeRangeRequestDto requestDto);
    Task<List<StatsMonthYearResponseDto>> GetStatsMonthYearSeriesAsync();
    #endregion
}
