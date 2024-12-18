namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasStatsUpdateHistoryResponseDto : IUpdateHistoryResponseDto
{
    DateTime OldStatsDateTime { get; }
    DateTime NewStatsDateTime { get; }

    string OldNote { get; }
    string NewNote { get; }
}