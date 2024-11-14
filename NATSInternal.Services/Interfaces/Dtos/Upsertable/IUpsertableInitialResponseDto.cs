namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IUpsertableInitialResponseDto : IInitialResponseDto
{
    bool CreatingPermission { get; }
}