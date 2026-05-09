using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Expenses;

[UsedImplicitly]
internal class ExpenseListValidator : AbstractListValidator<ExpenseListRequestDto, ExpenseListRequestDto.FieldToSort>;