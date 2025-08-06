namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpsertableInitialResponseDto : IInitialResponseDto
{
    bool CreatingPermission { get; }
}