namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasStatsUpsertRequestDto : IRequestDto
{
    string Note { get; }
    DateTime? StatsDateTime { get; }
    string UpdatedReason { get; }
}