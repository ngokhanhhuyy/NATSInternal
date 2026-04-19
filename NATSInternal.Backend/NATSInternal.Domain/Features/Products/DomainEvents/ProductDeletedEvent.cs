using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

public class ProductDeletedEvent : IDomainEvent
{
    #region Constructors
    internal ProductDeletedEvent(Guid id, DateTime deletedDateTime)
    {
        Id = id;
        DeletedDateTime = deletedDateTime;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public DateTime DeletedDateTime { get; }
    #endregion
}