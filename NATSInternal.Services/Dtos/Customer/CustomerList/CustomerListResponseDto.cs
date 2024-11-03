namespace NATSInternal.Services.Dtos;

public class CustomerListResponseDto
    : IUpsertableListResponseDto<
        CustomerBasicResponseDto,
        CustomerExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<CustomerBasicResponseDto> Items { get; set; }
}