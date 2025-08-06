namespace NATSInternal.Core.Entities;

internal class OrderUpdateHistory : IUpdateHistoryEntity<OrderUpdateHistory>
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
    public int OrderId { get; set; }

    [Required]
    public int UpdatedUserId { get; set; }

    // Navigation properties.
    public virtual Order Order { get; set; }
    public virtual User UpdatedUser { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<OrderUpdateHistory> entityBuilder)
    {
        entityBuilder.HasKey(ouh => ouh.Id);
        entityBuilder.HasOne(ouh => ouh.Order)
            .WithMany(o => o.UpdateHistories)
            .HasForeignKey(ouh => ouh.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(ouh => ouh.UpdatedUser)
            .WithMany(u => u.OrderUpdateHistories)
            .HasForeignKey(ouh => ouh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(ouh => ouh.UpdatedDateTime);
        entityBuilder.Property(ouh => ouh.OldData).HasColumnType("JSON");
        entityBuilder.Property(ouh => ouh.NewData).HasColumnType("JSON");
    }
}
