namespace NATSInternal.Core.Entities;

[Table("announcements")]
internal class Announcement : AbstractEntity<Announcement>, IUpsertableEntity<Announcement>
{
    #region Fields
    private User? _createdUser;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("category")]
    [Required]
    public AnnouncementCategory Category { get; set; } = AnnouncementCategory.Announcement;

    [Column("title")]
    [Required]
    [StringLength(AnnouncementContracts.TitleMaxLength)]
    public required string Title { get; set; }

    [Column("content")]
    [Required]
    [StringLength(AnnouncementContracts.ContentMaxLength)]
    public required string Content { get; set; }

    [Column("starting_datetime")]
    [Required]
    public DateTime StartingDateTime { get; set; }

    [Column("ending_datetime")]
    [Required]
    public DateTime EndingDateTime { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();
    #endregion

    #region ForeignKeyProperties
    [Column("created_user_id")]
    public Guid CreatedUserId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingField
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_createdUser))]
    public User CreatedUser
    {
        get => GetFieldOrThrowIfNull(_createdUser);
        set
        {
            CreatedUserId = value.Id;
            _createdUser = value;
        }
    }
    #endregion
}