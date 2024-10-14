namespace NATSInternal.Services.Dtos;

public class BrandListResponseDto : IUpsertableListResponseDto<
    BrandBasicResponseDto,
    BrandAuthorizationResponseDto,
    BrandListAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<BrandBasicResponseDto> Items { get; set; }
    public BrandListAuthorizationResponseDto Authorization { get; set; }
}
