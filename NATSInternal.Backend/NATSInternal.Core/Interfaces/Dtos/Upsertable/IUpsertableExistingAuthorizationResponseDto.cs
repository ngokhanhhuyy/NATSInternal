namespace NATSInternal.Core.Interfaces.Dtos;

public interface IUpsertableExistingAuthorizationResponseDto
{
    bool CanEdit { get; set; }
    bool CanDelete { get; set; }
}