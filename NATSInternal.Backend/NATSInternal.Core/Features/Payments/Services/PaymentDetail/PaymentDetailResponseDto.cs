using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Payments;

public class PaymentDetailResponseDto
{
    #region Constructors
    internal PaymentDetailResponseDto(Payment payment, PaymentExistingAuthorizationResponseDto authorization)
    {
        Id = payment.Id;
        StatsDate = payment.StatsDate;
        Type = payment.Type;
        Amount = payment.Amount;
        Note = payment.Note;
        Customer = new(payment.Customer);
        CreatedDateTime = payment.CreatedDateTime;
        CreatedUser = new(payment.CreatedUser);

        LastUpdateDateTime = payment.LastUpdatedDateTime;
        if (payment.LastUpdatedUser is not null)
        {
            LastUpdatedUser = new(payment.LastUpdatedUser);
        }

        DeletedDateTime = payment.DeletedDateTime;
        if (payment.DeletedUser is not null)
        {
            DeletedUser = new(payment.DeletedUser);
        }

        Authorization = authorization;
    }
    #endregion

    #region Properties
    public int Id { get; }
    public DateOnly StatsDate { get; }
    public PaymentType Type { get; }
    public long Amount { get; }
    public string? Note { get; }
    public CustomerBasicResponseDto Customer { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime? LastUpdateDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? DeletedDateTime { get; }
    public UserBasicResponseDto? DeletedUser { get; }
    public PaymentExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}