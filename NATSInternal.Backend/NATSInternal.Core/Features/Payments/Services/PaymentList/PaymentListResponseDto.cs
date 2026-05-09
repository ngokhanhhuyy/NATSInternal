using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Payments;

public class PaymentListResponseDto : IListResponseDto<PaymentBasicResponseDto>
{
    #region Constructors
    public PaymentListResponseDto(List<PaymentBasicResponseDto> items, int pageCount, int itemCount)
    {
        Items = items;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion
    #region Properties
    public List<PaymentBasicResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}