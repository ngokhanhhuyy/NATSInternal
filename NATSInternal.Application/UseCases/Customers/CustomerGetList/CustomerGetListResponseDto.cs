using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerGetListResponseDto : IPageableListResponseDto<CustomerBasicResponseDto>
{
    #region Constructors
    public CustomerGetListResponseDto(ICollection<CustomerBasicResponseDto> responseDtos, int pageCount)
    {
        Items = responseDtos;
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public ICollection<CustomerBasicResponseDto> Items { get; }
    public int PageCount { get; }
    #endregion
}