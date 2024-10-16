namespace NATSInternal.Services.Entities;

internal class User : IdentityUser<int>, IIdentifiableEntity<User>, IHasSinglePhotoEntity<User>
{
    [Required]
    [StringLength(10)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(10)]
    public string NormalizedFirstName { get; set; }

    [StringLength(20)]
    public string MiddleName { get; set; }

    [StringLength(20)]
    public string NormalizedMiddleName { get; set; }

    [Required]
    [StringLength(10)]
    public string LastName { get; set; }

    [Required]
    [StringLength(10)]
    public string NormalizedLastName { get; set; }

    [Required]
    [StringLength(45)]
    public string FullName { get; set; }

    [Required]
    [StringLength(45)]
    public string NormalizedFullName { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public DateOnly? JoiningDate { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    public DateTime? UpdatedDateTime { get; set; }

    [StringLength(255)]
    public string Note { get; set; }

    [StringLength(255)]
    public string AvatarUrl { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties.
    public virtual List<Role> Roles { get; set; }
    public virtual List<Customer> CreatedCustomers { get; set; }
    public virtual List<Supply> Supplies { get; set; }
    public virtual List<SupplyUpdateHistory> SupplyUpdateHistories { get; set; }
    public virtual List<Order> Orders { get; set; }
    public virtual List<OrderUpdateHistory> OrderUpdateHistories { get; set; }
    public virtual List<Treatment> CreatedTreatments { get; set; }
    public virtual List<Treatment> TreatmentsInCharge { get; set; }
    public virtual List<TreatmentUpdateHistory> TreatmentUpdateHistories { get; set; }
    public virtual List<Consultant> Consultants { get; set; }
    public virtual List<ConsultantUpdateHistory> ConsultantUpdateHistories { get; set; }
    public virtual List<Expense> Expenses { get; set; }
    public virtual List<ExpenseUpdateHistory> ExpenseUpdateHistories { get; set; }
    public virtual List<DebtIncurrence> Debts { get; set; }
    public virtual List<DebtIncurrenceUpdateHistory> DebtUpdateHistories { get; set; }
    public virtual List<DebtPayment> DebtPayments { get; set; }
    public virtual List<DebtPaymentUpdateHistory> DebtPaymentUpdateHistories { get; set; }
    public virtual List<Announcement> CreatedAnnouncements { get; set; }
    public virtual List<Notification> CreatedNotifications { get; set; }
    public virtual List<Notification> ReceivedNotifications { get; set; }
    public virtual List<Notification> ReadNotifications { get; set; }

    // Properties for convinience.
    [NotMapped]
    public Role Role => Roles
        .OrderByDescending(r => r.PowerLevel)
        .SingleOrDefault();

    [NotMapped]
    public int PowerLevel => Role.PowerLevel;

    [NotMapped]
    public List<Supply> UpdatedSupplies => SupplyUpdateHistories
        .Select(suh => suh.Supply)
        .ToList();

    [NotMapped]
    public List<Expense> UpdatedExpenses => ExpenseUpdateHistories
        .Select(euh => euh.Expense)
        .ToList();

    [NotMapped]
    public List<Order> UpdatedOrders => OrderUpdateHistories
        .Select(ouh => ouh.Order)
        .ToList();

    [NotMapped]
    public List<Treatment> UpdatedTreatments => TreatmentUpdateHistories
        .Select(tuh => tuh.Treatment)
        .ToList();

    [NotMapped]
    public List<DebtIncurrence> UpdatedDebts => DebtUpdateHistories
        .Select(duh => duh.DebtIncurrence)
        .ToList();

    [NotMapped]
    public List<DebtPayment> UpdatedDebtPayments => DebtPaymentUpdateHistories
        .Select(dpuh => dpuh.DebtPayment)
        .ToList();

    [NotMapped]
    public string ThumbnailUrl
    {
        get => AvatarUrl;
        set => AvatarUrl = value;
    }
    
    public bool HasPermission(string permissionName)
    {
        return Role.Claims
            .Any(r => r.ClaimType == "Permission" && r.ClaimValue == permissionName);
    }
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<User> entityBuilder)
    {
        entityBuilder.HasKey(u => u.Id);
        entityBuilder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(
                userRole => userRole
                    .HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId),
                userRole => userRole
                    .HasOne(ur => ur.User)
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId));
        entityBuilder.HasIndex(u => u.UserName)
            .IsUnique();
        entityBuilder.HasKey(u => u.Id);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}