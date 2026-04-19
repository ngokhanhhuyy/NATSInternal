using JetBrains.Annotations;
using NATSInternal.Application.Validation.Validators;

namespace NATSInternal.Application.UseCases.Supplies;

[UsedImplicitly]
internal class SupplyGetListValidator : AbstractListValidator<
    SupplyGetListRequestDto,
    SupplyGetListRequestDto.FieldToSort>;