using Bogus.Extensions;

namespace NATSInternal.Core.Entities;

[Table("users")]
internal class User : AbstractEntity<User>
{
    #region Fields
    private string _userName = string.Empty;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [BackingField(nameof(_userName))]
    [Column("user_name")]
    [Required]
    [StringLength(UserContracts.UserNameMaxLength)]
    public required string UserName
    {
        get => _userName;
        set
        {
            NormalizedUserName = value.RemoveDiacritics();
            _userName = value;
        }
    }

    [Column("normalized_user_name")]
    [Required]
    [StringLength(UserContracts.UserNameMaxLength)]
    public string NormalizedUserName { get; protected set; } = string.Empty;

    [Column("password_hash")]
    [Required]
    [StringLength(UserContracts.PasswordHashMaxLength)]
    public required string PasswordHash { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; protected set; }
    #endregion

    #region ConcurrencyOperationTrackingProperty
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    public List<Role> Roles { get; protected set; } = new();
    public List<Customer> CreatedCustomers { get; protected set; } = new();
    public List<Supply> Supplies { get; protected set; } = new();
    public List<Expense> Expenses { get; protected set; } = new();
    public List<Order> Orders { get; protected set; } = new();
    public List<Debt> Debts { get; protected set; } = new();
    public List<UpdateHistory> UpdateHistories { get; protected set; } = new();
    public List<UpdateHistory> OrderUpdateHistories { get; protected set; } = new();
    public List<Announcement> CreatedAnnouncements { get; protected set; } = new();
    public List<Notification> CreatedNotifications { get; protected set; } = new();
    public List<Notification> ReceivedNotifications { get; protected set; } = new();
    public List<Notification> ReadNotifications { get; protected set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public int PowerLevel => Roles.Max(r => r.PowerLevel);

    [NotMapped]
    public List<Supply> UpdatedSupplies => UpdateHistories
        .Where(uh => uh.UpdatedUserId == Id && uh.SupplyId.HasValue)
        .Select(uh => uh.Supply)
        .ToList();

    [NotMapped]
    public List<Expense> UpdatedExpenses => UpdateHistories
        .Where(uh => uh.UpdatedUserId == Id && uh.ExpenseId.HasValue)
        .Select(uh => uh.Expense)
        .ToList();

    [NotMapped]
    public List<Order> UpdatedOrders => UpdateHistories
        .Where(uh => uh.UpdatedUserId == Id && uh.OrderId.HasValue)
        .Select(uh => uh.Order)
        .ToList();

    [NotMapped]
    public List<Debt> UpdatedDebts => UpdateHistories
        .Where(uh => uh.UpdatedUserId == Id && uh.DebtId.HasValue)
        .Select(uh => uh.Debt)
        .ToList();
    #endregion

    #region Methods
    public bool HasPermission(string permissionName)
    {
        return Roles
            .SelectMany(r => r.Permissions)
            .Any(p => p.Name == permissionName);
    }
    #endregion
}