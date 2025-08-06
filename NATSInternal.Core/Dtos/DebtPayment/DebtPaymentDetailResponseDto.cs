namespace NATSInternal.Core.Dtos;

public class DebtPaymentDetailResponseDto
    : IDebtDetailResponseDto<
        DebtPaymentUpdateHistoryResponseDto,
        DebtPaymentExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public DebtPaymentExistingAuthorizationResponseDto Authorization { get; set; }
    public List<DebtPaymentUpdateHistoryResponseDto> UpdateHistories { get; set; }

    internal DebtPaymentDetailResponseDto(
            DebtPayment payment,
            DebtPaymentExistingAuthorizationResponseDto authorization)
    {
        Id = payment.Id;
        Amount = payment.Amount;
        Note = payment.Note;
        StatsDateTime = payment.StatsDateTime;
        CreatedDateTime = payment.CreatedDateTime;
        IsLocked = payment.IsLocked;
        Customer = new CustomerBasicResponseDto(payment.Customer);
        CreatedUser = new UserBasicResponseDto(payment.CreatedUser);
        Authorization = authorization;
        UpdateHistories = payment.UpdateHistories?
            .Select(uh => new DebtPaymentUpdateHistoryResponseDto(uh))
            .ToList();
    }
}