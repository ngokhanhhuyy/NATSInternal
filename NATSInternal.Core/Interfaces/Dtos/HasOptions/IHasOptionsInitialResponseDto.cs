namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasOptionsInitialResponseDto<TMinimal> where TMinimal : IMinimalResponseDto
{
    #region Properties
    List<TMinimal> AllAsOptions { get; }
    #endregion
}