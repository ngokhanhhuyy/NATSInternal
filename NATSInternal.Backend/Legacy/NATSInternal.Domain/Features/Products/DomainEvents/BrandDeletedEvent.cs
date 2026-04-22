using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

public class BrandDeletedEvent : IDomainEvent
{
    #region Constructors
    internal BrandDeletedEvent(Guid id, DateTime deletedDateTime)
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