using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Customers;

public class CustomerCreatedEvent : IDomainEvent
{
    #region Constructors
    internal CustomerCreatedEvent(CustomerSnapshot snapshot, DateTime createdDateTime)
    {
        Snapshot = snapshot;
        CreatedDateTime = createdDateTime;
    }
    #endregion

    #region Properties
    public CustomerSnapshot Snapshot { get; }
    public DateTime CreatedDateTime { get; }
    #endregion
}
