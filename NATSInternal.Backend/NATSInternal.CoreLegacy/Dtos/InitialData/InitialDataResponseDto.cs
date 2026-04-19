namespace NATSInternal.Core.Dtos;

public class InitialDataResponseDto
{
    public required Dictionary<string, string> DisplayNames { get; set; }
    public required AnnouncementInitialResponseDto Announcement { get; set; }
    public required BrandInitialResponseDto Brand { get; set; }
    public required ConsultantInitialResponseDto Consultant { get; set; }
    public required CountryInitialResponseDto Country { get; set; }
    public required CustomerInitialResponseDto Customer { get; set; }
    public required DebtInitialResponseDto Debt { get; set; }
    public required DebtPaymentInitialResponseDto DebtPayment { get; set; }
    public required ExpenseInitialResponseDto Expense { get; set; }
    public required OrderInitialResponseDto Order { get; set; }
    public required ProductInitialResponseDto Product { get; set; }
    public required ProductCategoryInitialResponseDto ProductCategory { get; set; }
    public required RoleInitialResponseDto Role { get; set; }
    public required SupplyInitialResponseDto Supply { get; set; }
    public required TreatmentInitialResponseDto Treatment { get; set; }
    public required UserInitialResponseDto User { get; set; }
    public required StatsInitialResponseDto Stats { get; set; }
}