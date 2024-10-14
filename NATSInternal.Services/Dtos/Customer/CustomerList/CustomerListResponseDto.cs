namespace NATSInternal.Services.Dtos;

public class CustomerListResponseDto
    : IUpsertableListResponseDto<
        CustomerBasicResponseDto,
        CustomerAuthorizationResponseDto,
        CustomerListAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<CustomerBasicResponseDto> Items { get; set; }
    public CustomerListAuthorizationResponseDto Authorization { get; set; }
}