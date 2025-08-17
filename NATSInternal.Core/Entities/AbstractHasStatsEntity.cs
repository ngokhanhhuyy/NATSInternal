namespace NATSInternal.Core.Entities;

internal abstract class AbstractHasStatsEntity<T, TUpdateHistory, TData>
    :
        AbstractUpsertableEntity<T>,
        IHasStatsEntity<T, TUpdateHistory, TData>
    where T : AbstractHasStatsEntity<T, TUpdateHistory, TData>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TData>
    where TData : class
{
    #region Fields
    private User? _createdUser;
    #endregion

    #region Properties
    [Column("note")]
    [Required]
    [StringLength(5000)]
    public string? Note { get; set; }

    [Column("stats_datetime")]
    [Required]
    public DateTime StatsDateTime { get; set; }

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ForeignKeyProperties
    public Guid CreatedUserId { get; set; }
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

    public List<TUpdateHistory> UpdateHistories { get; private set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public bool IsLocked
    {
        get
        {
            DateTime lockedDate = CreatedDateTime.AddMonths(2);
            DateTime lockedDateTime = new DateTime(lockedDate.Year, lockedDate.Month, 1, 0, 0, 0);
            DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
            return currentDateTime >= lockedDateTime;
        }
    }
    #endregion
}