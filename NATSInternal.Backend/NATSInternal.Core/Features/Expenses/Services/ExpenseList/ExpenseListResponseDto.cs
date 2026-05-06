using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Expenses;

public class ExpenseListResponseDto : IListResponseDto<ExpenseBasicResponseDto>
{
    #region Constructors
    public ExpenseListResponseDto(List<ExpenseBasicResponseDto> items, int pageCount, int itemCount)
    {
        Items = items;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public List<ExpenseBasicResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}