using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NATSInternal.Core.Features.Products;

internal class Product
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    [StringLength(ProductContracts.NameMaxLength)]
    public required string Name { get; set; }
    
    [StringLength(ProductContracts.DescriptionMaxLength)]
    public required string? Description { get; set; }

    [Required]
    [StringLength(ProductContracts.UnitMaxLength)]
    public required string Unit { get; set; }
    
    [Required]
    public required long DefaultAmountBeforeVatPerUnit { get; set; }
    
    [Required]
    public required int DefaultVatPercentagePerUnit { get; set; }
    
    [Required]
    public required bool IsForRetail { get; set; } = true; 
    
    [Required]
    public bool IsDiscontinued { get; set; }
    
    [Required]
    public required DateTime CreatedDateTime { get; set; }

    public DateTime? LastUpdatedDateTime { get; set; }

    public DateTime? DeletedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public int CreatedUserId { get; set; }

    public int? LastUpdatedUserId { get; set; }

    public int? DeletedUserId { get; set; }
    #endregion

    #region NavigationProperties
    public List<ProductCategory> Categories { get; private set; } = new();
    public Stock? Stock { get; set; }
    public User CreatedUser { get; set; } = null!;
    public User? LastUpdatedUser { get; set; }
    public User? DeletedUser { get; set; }
    public List<Photo> Photos { get; private set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public Photo? Thumbnail => Photos.SingleOrDefault(p => p.IsThumbnail);
    #endregion
}