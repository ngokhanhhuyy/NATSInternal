namespace NATSInternal.Services.Dtos;

public class DebtPaymentDetailResponseDto
    :
        IFinancialEngageableDetailResponseDto<
            DebtPaymentUpdateHistoryResponseDto,
            DebtPaymentAuthorizationResponseDto>,
        ICustomerEngageableBasicResponseDto<DebtPaymentAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime PaidDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public DebtPaymentAuthorizationResponseDto Authorization { get; set; }
    public List<DebtPaymentUpdateHistoryResponseDto> UpdateHistories { get; set; }

    internal DebtPaymentDetailResponseDto(
            DebtPayment payment,
            DebtPaymentAuthorizationResponseDto authorization,
            bool mapUpdateHistories = false)
    {
        Id = payment.Id;
        Amount = payment.Amount;
        Note = payment.Note;
        PaidDateTime = payment.PaidDateTime;
        CreatedDateTime = payment.CreatedDateTime;
        IsLocked = payment.IsLocked;
        Customer = new CustomerBasicResponseDto(payment.Customer);
        CreatedUser = new UserBasicResponseDto(payment.CreatedUser);
        Authorization = authorization;
        
        if (mapUpdateHistories)
        {
            UpdateHistories = payment.UpdateHistories
                .Select(uh => new DebtPaymentUpdateHistoryResponseDto(uh))
                .ToList();
        }
    }
}