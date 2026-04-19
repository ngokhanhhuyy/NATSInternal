namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasCustomerUpsertRequestDto : IHasStatsUpsertRequestDto
{
    #region Properties
    Guid CustomerId { get; }
    #endregion
}