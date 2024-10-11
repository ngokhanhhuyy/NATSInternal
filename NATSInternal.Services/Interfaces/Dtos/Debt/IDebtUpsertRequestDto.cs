namespace NATSInternal.Services.Interfaces.Dtos;

public interface IDebtUpsertRequestDto : ICustomerEngageableUpsertRequestDto
{
    long Amount { get; }
}