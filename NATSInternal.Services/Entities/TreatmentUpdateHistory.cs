namespace NATSInternal.Services.Entities;

internal class TreatmentUpdateHistory : IUpdateHistoryEntity<TreatmentUpdateHistory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime UpdatedDateTime { get; set; }

    [StringLength(255)]
    public string Reason { get; set; }

    [Column(TypeName = "JSON")]
    [StringLength(1000)]
    public string OldData { get; set; }

    [Column(TypeName = "JSON")]
    [Required]
    [StringLength(1000)]
    public string NewData { get; set; }

    // Foreign keys
    [Required]
    public int TreatmentId { get; set; }

    [Required]
    public int UpdatedUserId { get; set; }

    // Navigation properties.
    public virtual Treatment Treatment { get; set; }
    public virtual User UpdatedUser { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<TreatmentUpdateHistory> entityBuilder)
    {
        entityBuilder.HasKey(tuh => tuh.Id);
        entityBuilder.HasOne(tuh => tuh.Treatment)
            .WithMany(t => t.UpdateHistories)
            .HasForeignKey(tuh => tuh.TreatmentId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(tuh => tuh.UpdatedUser)
            .WithMany(u => u.TreatmentUpdateHistories)
            .HasForeignKey(tuh => tuh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(tuh => tuh.UpdatedDateTime);
    }
}
