using JetBrains.Annotations;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Debts;

[UsedImplicitly]
internal class DebtListValidator : Validator<DebtListRequestDto>
{
    #region Constructors
    public DebtListValidator(IClock clock)
    {
        Include(new HasStatsListValidator<DebtListRequestDto, DebtListRequestDto.FieldToSort>(clock));
    }
    #endregion
}