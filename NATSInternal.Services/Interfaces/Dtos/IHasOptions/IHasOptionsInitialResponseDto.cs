namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasOptionsInitialResponseDto<TMinimal> where TMinimal : IMinimalResponseDto
{
    List<TMinimal> AllAsOptions { get; }
}