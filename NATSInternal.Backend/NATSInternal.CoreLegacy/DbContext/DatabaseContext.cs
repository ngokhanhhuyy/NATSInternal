namespace NATSInternal.Core.DbContext;

internal class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Constructors
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    #endregion

    #region Properties
    public DbSet<User> Users { get; protected set; }
    public DbSet<Role> Roles { get; protected set; }
    public DbSet<UserRole> UserRoles { get; protected set; }
    public DbSet<Country> Countries { get; protected set; }
    public DbSet<Brand> Brands { get; protected set; }
    public DbSet<ProductCategory> ProductCategories { get; protected set; }
    public DbSet<Product> Products { get; protected set; }
    public DbSet<Customer> Customers { get; protected set; }
    public DbSet<Supply> Supplies { get; protected set; }
    public DbSet<SupplyItem> SupplyItems { get; protected set; }
    public DbSet<Order> Orders { get; protected set; }
    public DbSet<OrderItem> OrderItems { get; protected set; }
    public DbSet<UpdateHistory> OrderUpdateHistories { get; protected set; }
    public DbSet<Expense> Expenses { get; protected set; }
    public DbSet<ExpensePayee> ExpensePayees { get; protected set; }
    public DbSet<Debt> Debts { get; protected set; }
    public DbSet<Announcement> Announcements { get; protected set; }
    public DbSet<Notification> Notifications { get; protected set; }
    public DbSet<DailySummary> DailyStats { get; protected set; }
    public DbSet<MonthlySummary> MonthlyStats { get; protected set; }
    #endregion

    #region Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    #endregion
}