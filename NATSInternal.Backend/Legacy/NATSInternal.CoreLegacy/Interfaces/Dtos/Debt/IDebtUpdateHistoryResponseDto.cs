namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IDebtUpdateHistoryResponseDto : IHasStatsUpdateHistoryDataResponseDto
{
    long OldAmount { get; }
    long NewAmount { get; }
}