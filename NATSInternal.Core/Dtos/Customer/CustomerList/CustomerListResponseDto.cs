namespace NATSInternal.Core.Dtos;

public class CustomerListResponseDto
    : IUpsertableListResponseDto<CustomerBasicResponseDto, CustomerExistingAuthorizationResponseDto>
{
    #region Constructors
    internal CustomerListResponseDto(ICollection<Customer> customers, int pageCount)
    {
        Items.AddRange(customers.Select(c => new CustomerBasicResponseDto(c)));
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public int PageCount { get; set; }
    public List<CustomerBasicResponseDto> Items { get; set; } = new();
    #endregion
}