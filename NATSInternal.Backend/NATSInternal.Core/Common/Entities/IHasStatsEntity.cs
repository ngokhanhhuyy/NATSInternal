namespace NATSInternal.Core.Common.Entities;

internal interface IHasStatsEntity
{
    #region Properties
    DateOnly StatsDate { get; set; }
    #endregion
}