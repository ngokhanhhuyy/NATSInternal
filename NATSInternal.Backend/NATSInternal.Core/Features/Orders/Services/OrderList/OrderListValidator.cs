using JetBrains.Annotations;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Orders;

[UsedImplicitly]
internal class OrderListValidator : AbstractListValidator<OrderListRequestDto, OrderListRequestDto.FieldToSort>;