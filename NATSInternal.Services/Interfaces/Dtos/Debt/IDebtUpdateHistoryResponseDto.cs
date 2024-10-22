namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IDebtUpdateHistoryResponseDto : IFinancialEngageableUpdateHistoryResponseDto
{
    long OldAmount { get; }
    long NewAmount { get; }
}