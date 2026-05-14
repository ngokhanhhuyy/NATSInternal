using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;

namespace NATSInternal.Core.Features.Debts;

[UsedImplicitly]
internal class DebtCreateValidator : AbstractDebtUpsertValidator<DebtCreateRequestDto>
{
    #region Constructors
    public DebtCreateValidator(IClock clock) : base(clock)
    {
        RuleFor(dto => dto.CustomerId)
            .NotEmpty()
            .WithName(DisplayNames.Customer);
        RuleFor(dto => dto.OrderId)
            .NotEmpty()
            .WithName(DisplayNames.Order);
    }
    #endregion
}
