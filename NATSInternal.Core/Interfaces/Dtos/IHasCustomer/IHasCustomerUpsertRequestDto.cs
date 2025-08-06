namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasCustomerUpsertRequestDto : IHasStatsUpsertRequestDto
{
    int CustomerId { get; }
}