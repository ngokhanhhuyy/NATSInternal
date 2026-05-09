using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;

namespace NATSInternal.Core.Features.Payments;

[UsedImplicitly]
internal class PaymentCreateValidator : AbstractPaymentUpsertValidator<PaymentCreateRequestDto>
{
    #region Constructors
    public PaymentCreateValidator(IClock clock) : base(clock)
    {
        RuleFor(dto => dto.CustomerId)
            .GreaterThan(0)
            .WithName(DisplayNames.Customer);
        RuleFor(dto => dto.Type)
            .IsInEnum()
            .WithName(DisplayNames.PaymentType);
        RuleFor(dto => dto.OrderId)
            .GreaterThan(0)
            .WithName(DisplayNames.Order);
    }
    #endregion
}