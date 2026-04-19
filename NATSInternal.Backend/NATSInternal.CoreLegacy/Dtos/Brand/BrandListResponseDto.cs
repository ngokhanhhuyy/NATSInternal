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
    internal BrandListResponseDto(ICollection<BrandBasicResponseDto> responseDtos, int pageCount)
    {
        Items.AddRange(responseDtos);
        PageCount = pageCount;
    }
    #endregion
}