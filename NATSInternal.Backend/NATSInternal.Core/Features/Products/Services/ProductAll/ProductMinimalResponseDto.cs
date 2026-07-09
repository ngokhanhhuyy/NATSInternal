namespace NATSInternal.Core.Features.Products;

public class ProductMinimalResponseDto
{
    #region Constructors
    internal ProductMinimalResponseDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
    }
    #endregion

    #region Properties
    public int Id { get; set; }
    public string Name { get; set; }
    #endregion
}
