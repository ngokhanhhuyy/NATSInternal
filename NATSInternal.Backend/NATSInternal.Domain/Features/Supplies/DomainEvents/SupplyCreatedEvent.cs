using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Supplies;

public class SupplyCreatedEvent : IDomainEvent
{
    #region Constructors
    internal SupplyCreatedEvent(Supply supply)
    {
        Snapshot = new SupplySnapshot(supply);
        CreatedDateTime = supply.CreatedDateTime;
    }
    #endregion

    #region Properties
    public SupplySnapshot Snapshot { get; }
    public DateTime CreatedDateTime { get; }
    #endregion
}