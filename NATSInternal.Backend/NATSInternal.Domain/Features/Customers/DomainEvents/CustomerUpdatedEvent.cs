using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Customers;

public class CustomerUpdatedEvent : IDomainEvent
{
    #region Constructors
    internal CustomerUpdatedEvent(
        CustomerSnapshot beforeUpdatingSnapshot,
        CustomerSnapshot afterUpdatingSnapshot,
        DateTime updatedDateTime)
    {
        BeforeUpdatingSnapshot = beforeUpdatingSnapshot;
        AfterUpdatingSnapshot = afterUpdatingSnapshot;
        UpdatedDateTime = updatedDateTime;
    }
    #endregion

    #region Properties
    public CustomerSnapshot BeforeUpdatingSnapshot { get; }
    public CustomerSnapshot AfterUpdatingSnapshot { get; }
    public DateTime UpdatedDateTime { get; }
    #endregion
}
