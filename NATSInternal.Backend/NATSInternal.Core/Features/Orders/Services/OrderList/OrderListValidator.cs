using JetBrains.Annotations;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Orders;

[UsedImplicitly]
internal class OrderListValidator : Validator<OrderListRequestDto>
{
    #region Constructors
    public OrderListValidator(IClock clock)
    {
        Include(new HasStatsListValidator<OrderListRequestDto, OrderListRequestDto.FieldToSort>(clock));
    }
    #endregion
}