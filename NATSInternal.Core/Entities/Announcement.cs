namespace NATSInternal.Core.Entities;

internal class Announcement : IUpsertableEntity<Announcement>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public AnnouncementCategory Category { get; set; } = AnnouncementCategory.Announcement;

    [Required]
    [StringLength(80)]
    public string Title { get; set; }

    [Required]
    [StringLength(5000)]
    public string Content { get; set; }

    [Required]
    public DateTime StartingDateTime { get; set; }

    [Required]
    public DateTime EndingDateTime { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    // Foreign keys
    public int CreatedUserId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual User CreatedUser { get; set; }

    // Configurations.
    public static void ConfigureModel(EntityTypeBuilder<Announcement> entity)
    {
        entity.HasKey(a => a.Id);
        entity.HasOne(a => a.CreatedUser)
            .WithMany(u => u.CreatedAnnouncements)
            .HasForeignKey(a => a.CreatedUserId)
            .OnDelete(DeleteBehavior.Cascade);
        entity.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}