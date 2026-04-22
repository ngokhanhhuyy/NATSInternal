using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Supplies;

public class SupplyUpdatedEvent : IDomainEvent
{
    #region Constructors
    internal SupplyUpdatedEvent(Supply supply, DateTime updatedDateTime)
    {
        BeforeModificationSnapshot = new(supply);
        AfterModificationSnapshot = new(supply);
        UpdatedDateTime = updatedDateTime;
    }
    #endregion

    #region Properties
    public SupplySnapshot BeforeModificationSnapshot { get; }
    public SupplySnapshot AfterModificationSnapshot { get; }
    public DateTime UpdatedDateTime { get; }
    #endregion
}