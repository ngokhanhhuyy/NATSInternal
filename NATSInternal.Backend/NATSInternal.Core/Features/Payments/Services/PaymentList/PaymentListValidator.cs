using JetBrains.Annotations;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Payments;

[UsedImplicitly]
internal class PaymentListValidator : Validator<PaymentListRequestDto>
{
    #region Constructors
    public PaymentListValidator(IClock clock)
    {
        Include(new HasStatsListValidator<PaymentListRequestDto, PaymentListRequestDto.FieldToSort>(clock));
    }
    #endregion
}