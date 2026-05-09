using JetBrains.Annotations;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Expenses;

[UsedImplicitly]
internal class ExpenseListValidator : Validator<ExpenseListRequestDto>
{
    public ExpenseListValidator(IClock clock)
    {
        Include(new HasStatsListValidator<ExpenseListRequestDto, ExpenseListRequestDto.FieldToSort>(clock));
    }
}