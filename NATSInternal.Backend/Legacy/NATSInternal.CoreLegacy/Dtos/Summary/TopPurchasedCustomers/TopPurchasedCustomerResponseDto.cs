namespace NATSInternal.Core.Dtos;

public class TopPurchasedCustomerResponseDto
{
    #region Constructors
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
    #endregion

    #region Properties
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string? NickName { get; set; }
    public Gender Gender { get; set; }
    public long PurchasedAmount { get; set; }
    public int PurchasedTransactionCount { get; set; }
    #endregion
}
