using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Products;

internal class ProductCategory
{
    #region Properties
    [Key]
    public int Id { get; private set; }
    
    [Required]
    [StringLength(ProductCategoryContracts.NameMaxLength)]
    public required string Name { get; set; }
    #endregion

    #region NavigationProperties
    public List<Product> Products { get; private set; } = new();
    #endregion
}