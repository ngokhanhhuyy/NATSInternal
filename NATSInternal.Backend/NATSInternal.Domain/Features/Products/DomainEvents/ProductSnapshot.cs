namespace NATSInternal.Domain.Features.Products;

public class ProductSnapshot
{
    #region Constructors
    internal ProductSnapshot(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Unit = product.Unit;
        DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentage = product.DefaultVatPercentagePerUnit;
        IsForRetail = product.IsForRetail;
        IsDiscontinued = product.IsDiscontinued;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public string Unit { get; }
    public long DefaultAmountBeforeVatPerUnit { get; }
    public int DefaultVatPercentage { get; }
    public bool IsForRetail { get; }
    public bool IsDiscontinued { get; }
    #endregion
}