namespace NATSInternal.Domain.Features.Products;

public class BrandSnapshot
{
    #region Constructors
    internal BrandSnapshot(Brand brand)
    {
        
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    #endregion
}