namespace NATSInternal.Services.Dtos;

public class BrandListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}
