namespace NATSInternal.Services.Entities;

internal class ConsultantUpdateHistory : IUpdateHistoryEntity<ConsultantUpdateHistory>
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
    public int ConsultantId { get; set; }

    [Required]
    public int UpdatedUserId { get; set; }

    // Navigation properties
    public virtual Consultant Consultant { get; set; }
    public virtual User UpdatedUser { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<ConsultantUpdateHistory> entityBuilder)
    {
        entityBuilder.HasKey(cuh => cuh.Id);
        entityBuilder.HasOne(cuh => cuh.Consultant)
            .WithMany(c => c.UpdateHistories)
            .HasForeignKey(cuh => cuh.ConsultantId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(cuh => cuh.UpdatedUser)
            .WithMany(u => u.ConsultantUpdateHistories)
            .HasForeignKey(cuh => cuh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(cuh => cuh.UpdatedDateTime);
        entityBuilder.Property(cuh => cuh.OldData).HasColumnType("JSON");
        entityBuilder.Property(cuh => cuh.NewData).HasColumnType("JSON");
    }
}
