namespace NATSInternal.Core.Entities;

internal abstract class HasStatsAbstractEntity
{
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [NotMapped]
    public bool IsLocked
    {
        get
        {
            DateTime lockedMonthAndYear = CreatedDateTime
                .AddMonths(2);
            DateTime lockedDateTime = new DateTime(
                lockedMonthAndYear.Year, lockedMonthAndYear.Month, 1,
                0, 0, 0);
            DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
            return currentDateTime >= lockedDateTime;
        }
    }
}