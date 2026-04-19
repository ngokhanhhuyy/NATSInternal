using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

public class ProductCreatedEvent : IDomainEvent
{
    #region Constructors
    internal ProductCreatedEvent(Guid id, DateTime createdDateTime)
    {
        Id = id;
        CreatedDateTime = createdDateTime;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public DateTime CreatedDateTime { get; }
    #endregion
}
