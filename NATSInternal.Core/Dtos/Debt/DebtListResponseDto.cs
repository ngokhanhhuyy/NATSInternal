namespace NATSInternal.Core.Dtos;

public class DebtListResponseDto : IHasStatsResponseDto<DebtBasicResponseDto, DebtExistingAuthorizationResponseDto>
{
    #region Properties
    public List<DebtBasicResponseDto> Items { get; set; } = new();
    public int PageCount { get; set; }
    #endregion
    
    #region Constructors
    internal DebtListResponseDto() { }
    
    internal DebtListResponseDto(IEnumerable<DebtBasicResponseDto> basicResponseDtos, int pageCount)
    {
        Items.AddRange(basicResponseDtos);
        PageCount = pageCount;
    }
    #endregion
}