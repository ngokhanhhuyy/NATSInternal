namespace NATSInternal.Core.Dtos;

public class StatsInitialResponseDto
{
    public required TopSoldProductInitialResponseDto TopSoldProduct { get; set; }
    public required TopPurchasedInitialResponseDto TopPurchasedCustomer { get; set; }
}