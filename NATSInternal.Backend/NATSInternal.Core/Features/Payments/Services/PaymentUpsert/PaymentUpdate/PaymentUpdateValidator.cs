using JetBrains.Annotations;
using NATSInternal.Core.Common.Time;

namespace NATSInternal.Core.Features.Payments;

[UsedImplicitly]
internal class PaymentUpdateValidator : AbstractPaymentUpsertValidator<PaymentUpdateRequestDto>
{
    #region Constructors
    public PaymentUpdateValidator(IClock clock) : base(clock) { }
    #endregion
}