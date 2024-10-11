namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableUpsertRequestDto : IRequestDto
{
    string Note { get; }
    DateTime? StatsDateTime { get; }
    string UpdatedReason { get; }
}