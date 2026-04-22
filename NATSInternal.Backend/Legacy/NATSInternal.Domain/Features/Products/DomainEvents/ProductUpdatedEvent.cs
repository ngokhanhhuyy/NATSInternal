using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

public class ProductUpdatedEvent : IDomainEvent
{
    #region Constructors
    internal ProductUpdatedEvent(Guid id, DateTime updatedDateTime)
    {
        Id = id;
        UpdatedDateTime = updatedDateTime;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public DateTime UpdatedDateTime { get; }
    #endregion
}