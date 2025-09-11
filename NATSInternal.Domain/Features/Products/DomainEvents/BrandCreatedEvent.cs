using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

public class BrandCreatedEvent : IDomainEvent
{
    #region Properties
    public Guid CreatedBrandId { get; set; }
    
    #endregion
}