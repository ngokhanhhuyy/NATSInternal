namespace NATSInternal.Core.Entities;

internal class DebtPaymentUpdateHistory : IUpdateHistoryEntity<DebtPaymentUpdateHistory>
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
    public int DebtPaymentId { get; set; }

    [Required]
    public int UpdatedUserId { get; set; }

    // Navigation properties
    public virtual DebtPayment DebtPayment { get; set; }
    public virtual User UpdatedUser { get; set; }
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<DebtPaymentUpdateHistory> builder)
    {
        builder.HasKey(dpuh => dpuh.Id);
        builder.HasOne(dpuh => dpuh.DebtPayment)
            .WithMany(dp => dp.UpdateHistories)
            .HasForeignKey(dp => dp.DebtPaymentId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(dpuh => dpuh.UpdatedUser)
            .WithMany(u => u.DebtPaymentUpdateHistories)
            .HasForeignKey(dpuh => dpuh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(dpuh => dpuh.UpdatedDateTime);
        builder.Property(dpuh => dpuh.OldData).HasColumnType("JSON");
        builder.Property(dpuh => dpuh.NewData).HasColumnType("JSON");
    }
}
