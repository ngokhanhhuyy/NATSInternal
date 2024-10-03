namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableAuthorizationResponseDto
{
    bool CanEdit { get; internal set; }
    bool CanDelete { get; internal set; }
}