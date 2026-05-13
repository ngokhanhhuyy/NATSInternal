using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Payments;

public class PaymentBasicResponseDto
{
    #region Constructors
    internal PaymentBasicResponseDto(Payment payment)
    {
        Id = payment.Id;
        Type = payment.Type;
        StatsDate = payment.StatsDate;
        Amount = payment.Amount;
        CustomerId = payment.CustomerId;
    }

    internal PaymentBasicResponseDto(
        Payment payment,
        PaymentExistingAuthorizationResponseDto authorization) : this(payment)
    {
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public int Id { get; }
    public PaymentType Type { get; }
    public DateOnly StatsDate { get; }
    public long Amount { get; }
    public int CustomerId { get; }
    public PaymentExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}
