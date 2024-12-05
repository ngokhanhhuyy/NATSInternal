namespace NATSInternal.Services.Dtos;

public class TopPurchasedCustomerResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string NickName { get; set; }
    public Gender Gender { get; set; }
    public long PurchasedAmount { get; set; }
    public int PurchasedTransactionCount { get; set; }

    internal TopPurchasedCustomerResponseDto(
            Customer customer,
            long? purchasedAmount = null,
            int? purchasedTransactionCount = null)
    {
        Id = customer.Id;
        FullName = customer.FullName;
        NickName = customer.NickName;
        Gender = customer.Gender;

        if (purchasedAmount.HasValue)
        {
            PurchasedAmount = purchasedAmount.Value;
        }
        
        if (purchasedTransactionCount.HasValue)
        {
            PurchasedTransactionCount = purchasedTransactionCount.Value;
        }
    }
}
