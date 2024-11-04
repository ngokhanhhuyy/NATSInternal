namespace NATSInternal.Services.Interfaces.Dtos;

public interface IUpsertableExistingAuthorizationResponseDto
{
    bool CanEdit { get; set; }
    bool CanDelete { get; set; }
}