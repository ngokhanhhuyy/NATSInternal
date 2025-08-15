namespace NATSInternal.Core.Entities;

internal class Announcement : IUpsertableEntity<Announcement>
{
    #region Fields
    private User? _createdUser;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public AnnouncementCategory Category { get; set; } = AnnouncementCategory.Announcement;

    [Required]
    [StringLength(AnnouncementContracts.TitleMaxLength)]
    public required string Title { get; set; }

    [Required]
    [StringLength(AnnouncementContracts.ContentMaxLength)]
    public required string Content { get; set; }

    [Required]
    public DateTime StartingDateTime { get; set; }

    [Required]
    public DateTime EndingDateTime { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();
    #endregion

    #region ForeignKeyProperties
    public Guid CreatedUserId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingField
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    public User CreatedUser
    {
        get => _createdUser ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(CreatedUser)));
        set => _createdUser = value;
    }
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Announcement> entity)
    {
        entity.HasKey(a => a.Id);
        entity.HasOne(a => a.CreatedUser)
            .WithMany(u => u.CreatedAnnouncements)
            .HasForeignKey(a => a.CreatedUserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entity.Property(c => c.RowVersion).IsRowVersion();
    }
    #endregion
}