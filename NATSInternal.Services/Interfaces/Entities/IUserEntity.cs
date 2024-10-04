namespace NATSInternal.Services.Interfaces.Entities;

internal interface IUserEntity : IEntity
{
    string FirstName { get; set; }
    string NormalizedFirstName { get; set; }
    string MiddleName { get; set; }
    string NormalizedMiddleName { get; set; }
    string LastName { get; set; }
    string NormalizedLastName { get; set; }
    string FullName { get; set; }
    string NormalizedFullName { get; set; }
    Gender Gender { get; set; }
    DateOnly? Birthday { get; set; }
    DateOnly? JoiningDate { get; set; }
    DateTime CreatedDateTime { get; set; }
    DateTime? UpdatedDateTime { get; set; }
    string Note { get; set; }
    string AvatarUrl { get; set; }
    bool IsDeleted { get; set; }
    byte[] RowVersion { get; set; }

    // Navigation properties.
    List<Role> Roles { get; set; }
    List<UserRefreshToken> RefreshTokens { get; set; }
    List<Customer> CreatedCustomers { get; set; }
    List<Supply> Supplies { get; set; }
    List<SupplyUpdateHistory> SupplyUpdateHistories { get; set; }
    List<Order> Orders { get; set; }
    List<OrderUpdateHistory> OrderUpdateHistories { get; set; }
    List<Treatment> CreatedTreatments { get; set; }
    List<Treatment> TreatmentsInCharge { get; set; }
    List<TreatmentUpdateHistory> TreatmentUpdateHistories { get; set; }
    List<Consultant> Consultants { get; set; }
    List<ConsultantUpdateHistory> ConsultantUpdateHistories { get; set; }
    List<Expense> Expenses { get; set; }
    List<ExpenseUpdateHistory> ExpenseUpdateHistories { get; set; }
    List<DebtIncurrence> Debts { get; set; }
    List<DebtIncurrenceUpdateHistory> DebtUpdateHistories { get; set; }
    List<DebtPayment> DebtPayments { get; set; }
    List<DebtPaymentUpdateHistory> DebtPaymentUpdateHistories { get; set; }
    List<Announcement> CreatedAnnouncements { get; set; }
    List<Notification> CreatedNotifications { get; set; }
    List<Notification> ReceivedNotifications { get; set; }
    List<Notification> ReadNotifications { get; set; }

    // Properties for convinience.
    Role Role { get; }
    int PowerLevel { get; }
    List<Supply> UpdatedSupplies { get; }
    List<Expense> UpdatedExpenses { get; }
    List<Order> UpdatedOrders { get; }
    List<Treatment> UpdatedTreatments { get; }
    List<DebtIncurrence> UpdatedDebts { get; }
    List<DebtPayment> UpdatedDebtPayments { get; }

    bool HasPermission(string permissionName);
}