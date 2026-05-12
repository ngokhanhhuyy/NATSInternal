using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Debts;

public class DebtListResponseDto : IListResponseDto<DebtBasicResponseDto>
{
    #region Constructors
    public DebtListResponseDto(List<DebtBasicResponseDto> items, int pageCount, int itemCount)
    {
        Items = items;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion
    #region Properties
    public List<DebtBasicResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}