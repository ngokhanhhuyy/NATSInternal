namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICustomerEngageableUpsertRequestDto : IFinancialEngageableUpsertRequestDto
{
    int CustomerId { get; }
}