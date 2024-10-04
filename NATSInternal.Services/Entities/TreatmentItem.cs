namespace NATSInternal.Services.Entities;

[Table("treatment_items")]
internal class TreatmentItem
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("amount_before_vat_per_unit")]
    [Required]
    public long AmountBeforeVatPerUnit { get; set; }

    [Column("vat_amount_per_unit")]
    [Required]
    public long VatAmountPerUnit { get; set; }

    [Column("quantity")]
    [Required]
    public int Quantity { get; set; }

    // Foreign keys
    [Column("treatment_id")]
    [Required]
    public int TreatmentId { get; set; }

    [Column("product_id")]
    [Required]
    public int ProductId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Relationship
    public virtual Treatment Treatment { get; set; }
    public virtual Product Product { get; set; }

    // Properties for convinience.
    [NotMapped]
    public long AmountBeforeVat => AmountBeforeVatPerUnit * Quantity;

    [NotMapped]
    public long VatAmount => VatAmountPerUnit * Quantity;
}