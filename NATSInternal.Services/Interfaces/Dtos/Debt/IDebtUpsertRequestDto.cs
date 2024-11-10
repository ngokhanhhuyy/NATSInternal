namespace NATSInternal.Services.Interfaces.Dtos;

public interface IDebtUpsertRequestDto : IHasCustomerUpsertRequestDto
{
    long Amount { get; }
}