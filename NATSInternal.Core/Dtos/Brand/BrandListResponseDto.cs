namespace NATSInternal.Core.Dtos;

public class BrandListResponseDto : IUpsertableListResponseDto<
        BrandBasicResponseDto,
        BrandExistingAuthorizationResponseDto>
{
    #region Properties
    public List<BrandBasicResponseDto> Items { get; set; } = new();
    public int PageCount { get; set; }
    #endregion

    #region Constructors
    internal BrandListResponseDto(ICollection<Brand> brands, int pageCount)
    {
        Items.AddRange(brands.Select(b => new BrandBasicResponseDto(b)));
        PageCount = pageCount;
    }
    #endregion
}