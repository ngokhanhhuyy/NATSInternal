using NATSInternal.Application.UseCases.Customers;

namespace NATSInternal.Application.Services;

public interface ICustomerService
{
    #region Methods
    Task<CustomerGetListResponseDto> GetPaginatedCustomerListAsync(
        CustomerGetListRequestDto requestDto,
        CancellationToken cancellationToken = default);
    #endregion
}