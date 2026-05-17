using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
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
        
        RuleFor(dto => dto.CustomerId)
            .GreaterThan(0)
            .WithName(DisplayNames.Customer);
    }
    #endregion
}
