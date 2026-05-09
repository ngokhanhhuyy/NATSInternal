using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Customers;

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
    }

    internal PaymentBasicResponseDto(
        Payment payment,
        PaymentExistingAuthorizationResponseDto authorization) : this(payment)
    {
        Customer = new(payment.Customer);
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public int Id { get; }
    public PaymentType Type { get; }
    public DateOnly StatsDate { get; }
    public long Amount { get; }
    public CustomerBasicResponseDto? Customer { get; }
    public PaymentExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}