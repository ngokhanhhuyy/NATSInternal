namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableAuthorizationResponseDto
{
    bool CanEdit { get; }
    bool CanDelete { get; }
}