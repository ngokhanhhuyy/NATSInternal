namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableAuthorizationResponseDto
{
    bool CanEdit { get; set; }
    bool CanDelete { get; set; }
}