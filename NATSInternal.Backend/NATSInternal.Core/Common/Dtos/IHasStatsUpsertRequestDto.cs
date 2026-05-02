namespace NATSInternal.Core.Common.Dtos;

public interface IHasStatsUpsertRequestDto : IRequestDto
{
    #region Properties
    DateTime? StatsDateTime { get; set; }
    #endregion
}