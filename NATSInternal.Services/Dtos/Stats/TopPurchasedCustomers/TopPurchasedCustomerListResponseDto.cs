namespace NATSInternal.Services.Dtos;

public class TopPurchasedCustomerListResponseDto
{
    public required DateOnly StartingDate { get; set; }
    public required DateOnly EndingDate { get; set; }
    public List<TopPurchasedCustomerResponseDto> Items { get; set; }
}
