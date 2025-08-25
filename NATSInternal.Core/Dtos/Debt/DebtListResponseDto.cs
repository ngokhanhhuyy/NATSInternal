namespace NATSInternal.Core.Dtos;

public class DebtListResponseDto : IHasStatsResponseDto<DebtBasicResponseDto, DebtExistingAuthorizationResponseDto>
{
    #region Properties
    public List<DebtBasicResponseDto> Items { get; set; } = new();
    public int PageCount { get; set; }
    #endregion
    
    #region Constructors
    internal DebtListResponseDto() { }
    
    internal DebtListResponseDto(IEnumerable<Debt> debts, int pageCount)
    {
        Items.AddRange(debts.Select(d => new DebtBasicResponseDto(d)));
        PageCount = pageCount;
    }
    #endregion
}