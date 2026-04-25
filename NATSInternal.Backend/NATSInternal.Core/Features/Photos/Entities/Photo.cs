using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Photos;

internal class Photo
{
    #region Properties
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(PhotoContracts.UrlMaxLength)]
    public required string Url { get; set; }
    
    [Required]
    public required bool IsThumbnail { get; set; }

    public int? ProductId { get; set; }
    public int? SupplyId { get; set; }
    public int? ExpenseId { get; set; }
    public int? OrderId { get; set; }
    #endregion
}