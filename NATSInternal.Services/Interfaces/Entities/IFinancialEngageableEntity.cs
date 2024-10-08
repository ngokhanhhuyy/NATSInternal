namespace NATSInternal.Services.Interfaces.Entities;

internal interface IFinancialEngageableEntity<T, TUpdateHistory>
    :
        ICreatorTrackableEntity<T>,
        IUpdaterTrackableEntity<T, TUpdateHistory>
    where T : class, IUpsertableEntity<T>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    DateTime StatsDateTime { get; set; }
    string Note { get; set; }

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

    abstract static Expression<Func<T, DateTime>> StatsDateTimeExpression { get; }
}