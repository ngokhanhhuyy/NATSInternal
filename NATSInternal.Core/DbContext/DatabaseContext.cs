namespace NATSInternal.Core.DbContext;

internal class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Constructors
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    #endregion

    #region Properties
    public DbSet<Country> Countries { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Supply> Supplies { get; set; }
    public DbSet<SupplyItem> SupplyItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<UpdateHistory> OrderUpdateHistories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpensePayee> ExpensePayees { get; set; }
    public DbSet<Debt> Debts { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<DailySummary> DailyStats { get; set; }
    public DbSet<MonthlySummary> MonthlyStats { get; set; }
    #endregion

    #region Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    #endregion
}