using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

public class BrandCreatedEvent : IDomainEvent
{
    #region Constructors
    public BrandCreatedEvent(Guid createdBrandId, string createdBrandName)
    {
        CreatedBrandId = createdBrandId;
        CreatedBrandName = createdBrandName;
    }
    #endregion
    
    #region Properties
    public Guid CreatedBrandId { get; }
    public string CreatedBrandName { get; }
    #endregion
}