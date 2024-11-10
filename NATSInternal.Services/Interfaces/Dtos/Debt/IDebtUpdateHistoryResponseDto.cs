namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IDebtUpdateHistoryResponseDto : IHasStatsUpdateHistoryResponseDto
{
    long OldAmount { get; }
    long NewAmount { get; }
}