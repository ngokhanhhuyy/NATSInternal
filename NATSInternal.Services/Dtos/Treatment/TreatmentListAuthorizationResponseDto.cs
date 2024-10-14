namespace NATSInternal.Services.Dtos;

public class TreatmentListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}