using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Supplies;

[UsedImplicitly]
internal class SupplyListValidator : AbstractListValidator<SupplyListRequestDto, SupplyListRequestDto.FieldToSort>;