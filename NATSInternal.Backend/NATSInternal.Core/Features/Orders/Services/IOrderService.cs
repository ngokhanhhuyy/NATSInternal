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
    Task<CountResponseDto<long>> GetRevenueAsync(CountRequestDto requestDto);
    Task<CountResponseDto<int>> GetCountAsync(CountRequestDto requestDto);
    Task<CountResponseDto<int>> GetConsultantCountAsync(CountRequestDto requestDto);
    Task<CountResponseDto<int>> GetRetailCountAsync(CountRequestDto requestDto);
    Task<CountResponseDto<int>> GetTreatmentCountAsync(CountRequestDto requestDto);
    Task<List<StatsMonthYearResponseDto>> GetStatsMonthYearSeriesAsync();
    #endregion
}
