namespace NATSInternal.Core.Entities;

internal class DebtIncurrenceUpdateHistory : IUpdateHistoryEntity<DebtIncurrenceUpdateHistory>
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
    public int DebtIncurrenceId { get; set; }

    [Required]
    public int UpdatedUserId { get; set; }

    // Navigation properties
    public virtual DebtIncurrence DebtIncurrence { get; set; }
    public virtual User UpdatedUser { get; set; }
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<DebtIncurrenceUpdateHistory> builder)
    {
        builder.HasKey(duh => duh.Id);
        builder.HasOne(duh => duh.DebtIncurrence)
            .WithMany(d => d.UpdateHistories)
            .HasForeignKey(duh => duh.DebtIncurrenceId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(duh => duh.UpdatedUser)
            .WithMany(u => u.DebtUpdateHistories)
            .HasForeignKey(duh => duh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(duh => duh.UpdatedDateTime);
        builder.Property(duh => duh.OldData).HasColumnType("JSON");
        builder.Property(duh => duh.NewData).HasColumnType("JSON");
    }
}
