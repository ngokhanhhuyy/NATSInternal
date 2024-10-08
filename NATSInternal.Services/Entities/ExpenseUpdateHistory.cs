namespace NATSInternal.Services.Entities;

internal class ExpenseUpdateHistory : IUpdateHistoryEntity<ExpenseUpdateHistory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime UpdatedDateTime { get; set; }

    [StringLength(255)]
    public string Reason { get; set; }

    [StringLength(1000)]
    public string OldData { get; set; }

    [Required]
    [StringLength(1000)]
    public string NewData { get; set; }

    // Foreign keys
    [Required]
    public int ExpenseId { get; set; }

    [Required]
    public int UpdatedUserId { get; set; }

    // Navigation properties
    public virtual Expense Expense { get; set; }
    public virtual User UpdatedUser { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<ExpenseUpdateHistory> entityBuilder)
    {
        entityBuilder.HasKey(euh => euh.Id);
        entityBuilder.HasOne(euh => euh.Expense)
            .WithMany(ex => ex.UpdateHistories)
            .HasForeignKey(euh => euh.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(euh => euh.UpdatedUser)
            .WithMany(u => u.ExpenseUpdateHistories)
            .HasForeignKey(euh => euh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(euh => euh.UpdatedDateTime);
        entityBuilder.Property(euh => euh.OldData).HasColumnType("JSON");
        entityBuilder.Property(euh => euh.NewData).HasColumnType("JSON");
    }
}
