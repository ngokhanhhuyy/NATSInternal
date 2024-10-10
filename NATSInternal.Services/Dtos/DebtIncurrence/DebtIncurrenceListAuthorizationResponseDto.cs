namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceListAuthorizationResponseDto
    : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}