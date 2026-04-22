using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Supplies;

public class SupplyDeletedEvent : IDomainEvent
{
    #region Constructors
    internal SupplyDeletedEvent(Supply supply, DateTime deletedDateTime)
    {
        BeforeDeletionSnapshot = new(supply);
        DeletedDateTime = deletedDateTime;
    }
    #endregion

    #region Properties
    public SupplySnapshot BeforeDeletionSnapshot { get; }
    public DateTime DeletedDateTime { get; }
    #endregion
}