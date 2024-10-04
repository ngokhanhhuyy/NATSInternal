namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICustomerFinancialEngageableListRequestDto<TRequestDto>
    : IListRequestDto<TRequestDto>
{
    int? CustomerId { get; internal set; }
}