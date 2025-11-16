using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Customers;

public class CustomerDeletedEvent : IDomainEvent
{
    #region Constructors
    internal CustomerDeletedEvent(CustomerSnapshot beforeDeletingSnapshot, DateTime deletedDateTime)
    {
        BeforeDeletingSnapshot = beforeDeletingSnapshot;
        DeletedDateTime = deletedDateTime;
    }
    #endregion

    #region Properties
    public CustomerSnapshot BeforeDeletingSnapshot { get; }
    public DateTime DeletedDateTime { get; }
    #endregion
}
