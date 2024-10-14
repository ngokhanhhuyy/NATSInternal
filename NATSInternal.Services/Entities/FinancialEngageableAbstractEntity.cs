namespace NATSInternal.Services.Entities;

internal abstract class FinancialEngageableAbstractEntity
{
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [NotMapped]
    public bool IsLocked
    {
        get
        {
            DateTime lockedMonthAndYear = CreatedDateTime
                .AddMonths(2)
                .AddDays(-CreatedDateTime.Day)
                .AddHours(-CreatedDateTime.Hour);
            DateTime lockedDateTime = new DateTime(
                lockedMonthAndYear.Year, lockedMonthAndYear.Month, 1,
                0, 0, 0);
            DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
            return currentDateTime >= lockedDateTime;
        }
    }
}