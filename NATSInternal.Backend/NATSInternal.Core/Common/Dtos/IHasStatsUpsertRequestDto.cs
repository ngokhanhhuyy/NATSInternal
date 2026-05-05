namespace NATSInternal.Core.Common.Dtos;

public interface IHasStatsUpsertRequestDto : IRequestDto
{
    #region Properties
    DateOnly? StatsDate { get; set; }
    #endregion
}