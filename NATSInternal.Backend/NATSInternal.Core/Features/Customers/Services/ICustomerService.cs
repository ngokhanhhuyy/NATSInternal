namespace NATSInternal.Core.Features.Customers.Services;

public interface ICustomerService
{
    #region Properties
    Task<CustomerListResponseDto> GetListAsync(CustomerListRequestDto requestDto);
    #endregion
}