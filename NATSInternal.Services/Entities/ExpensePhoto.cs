namespace NATSInternal.Services.Entities;

internal class ExpensePhoto : IPhotoEntity<ExpensePhoto>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Url { get; set; }

    // Foreign key
    [Required]
    public int ExpenseId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual Expense Expense { get; set; }

    // Model configuration.
    public static void ConfigureModel(EntityTypeBuilder<ExpensePhoto> entityBuilder)
    {
        entityBuilder.HasKey(ep => ep.Id);
        entityBuilder.HasOne(p => p.Expense)
            .WithMany(ex => ex.Photos)
            .HasForeignKey(ex => ex.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasIndex(p => p.Url)
            .IsUnique();
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}