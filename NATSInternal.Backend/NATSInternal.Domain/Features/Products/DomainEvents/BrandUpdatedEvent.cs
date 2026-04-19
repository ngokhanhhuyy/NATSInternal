using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

public class BrandUpdatedEvent : IDomainEvent
{
    #region Constructors
    public BrandUpdatedEvent(Guid id, DateTime updatedDateTime)
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