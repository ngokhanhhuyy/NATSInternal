namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasCustomerUpsertRequestDto : IHasStatsUpsertRequestDto
{
    int CustomerId { get; }
}