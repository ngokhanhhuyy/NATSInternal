namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IMinimalResponseDto
{
    #region Properties
    Guid Id { get; }
    string Name { get; }
    #endregion
}