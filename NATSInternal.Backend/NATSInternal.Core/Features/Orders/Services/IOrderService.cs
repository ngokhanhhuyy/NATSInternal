namespace NATSInternal.Core.Features.Orders;

public interface IOrderService
{
    #region Methods
    Task<OrderListResponseDto> GetListAsync(OrderListRequestDto requestDto);
    Task<OrderDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(OrderUpsertRequestDto requestDto);
    Task UpdateAsync(int id, OrderUpsertRequestDto requestDto);
    Task DeleteAsync(int id);
    #endregion
}