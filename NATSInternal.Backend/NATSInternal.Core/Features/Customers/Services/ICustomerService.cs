using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Customers;

public interface ICustomerService
{
    #region Properties
    Task<CustomerListResponseDto> GetListAsync(CustomerListRequestDto requestDto);
    Task<CustomerDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(CustomerUpsertRequestDto requestDto);
    Task UpdateAsync(int id, CustomerUpsertRequestDto requestDto);
    Task DeleteAsync(int id);
    Task<int> GetCountAsync();
    Task<int> GetHavingDebtCountAsync();
    Task<int> GetRefundNeededCountAsync();
    Task<CountOverTimeRangeResponseDto<int>> GetNewCountAsync(CountOverTimeRangeRequestDto requestDto);
    Task<CountOverTimeRangeResponseDto<int>> GetPurchasedCountAsync(CountOverTimeRangeRequestDto requestDto);
    #endregion
}
