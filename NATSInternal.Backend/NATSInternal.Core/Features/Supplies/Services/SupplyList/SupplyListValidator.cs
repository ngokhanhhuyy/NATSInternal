using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Supplies;

[UsedImplicitly]
internal class SupplyGetListValidator : AbstractListValidator<SupplyListRequestDto, SupplyListRequestDto.FieldToSort>;