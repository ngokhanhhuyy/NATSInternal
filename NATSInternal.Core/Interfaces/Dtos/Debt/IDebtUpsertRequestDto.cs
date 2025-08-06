namespace NATSInternal.Core.Interfaces.Dtos;

public interface IDebtUpsertRequestDto : IHasCustomerUpsertRequestDto
{
    long Amount { get; }
}