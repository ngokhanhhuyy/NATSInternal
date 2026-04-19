namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasStatsUpdateHistoryDataResponseDto : IUpdateHistoryDataResponseDto
{
    #region Constructors
    DateTime OldStatsDateTime { get; }
    DateTime NewStatsDateTime { get; }

    string? OldNote { get; }
    string? NewNote { get; }
    #endregion
}