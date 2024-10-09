namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableUpsertRequestDto : IRequestDto
{
    long Amount { get; }
    string Note { get; }
    DateTime? StatsDateTime { get; }
    string UpdatedReason { get; }
}