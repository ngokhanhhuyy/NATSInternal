namespace NATSInternal.Core.Entities;

internal class SupplyUpdateHistory : IUpdateHistoryEntity<SupplyUpdateHistory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime UpdatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [StringLength(255)]
    public string Reason { get; set; }

    [StringLength(1000)]
    public string OldData { get; set; }

    [Required]
    [StringLength(1000)]
    public string NewData { get; set; }

    // Foreign keys
    [Required]
    public int SupplyId { get; set; }

    [Required]
    public int UpdatedUserId { get; set; }

    // Navigation properties
    public virtual Supply Supply { get; set; }
    public virtual User UpdatedUser { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<SupplyUpdateHistory> entityBuilder)
    {
        entityBuilder.HasKey(suh => suh.Id);
        entityBuilder.HasOne(suh => suh.Supply)
            .WithMany(s => s.UpdateHistories)
            .HasForeignKey(suh => suh.SupplyId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(suh => suh.UpdatedUser)
            .WithMany(u => u.SupplyUpdateHistories)
            .HasForeignKey(suh => suh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(suh => suh.UpdatedDateTime);
        entityBuilder.Property(p => p.OldData).HasColumnType("JSON");
        entityBuilder.Property(p => p.NewData).HasColumnType("JSON");
    }
}
