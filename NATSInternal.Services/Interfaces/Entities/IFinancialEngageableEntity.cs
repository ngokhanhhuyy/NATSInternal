namespace NATSInternal.Services.Interfaces.Entities;

internal interface IFinancialEngageableEntity<T, TUser, TUpdateHistory>
    :
        ICreatorTrackableEntity<T, TUser>,
        IUpdaterTrackableEntity<T, TUser, TUpdateHistory>
    where T : class, IUpsertableEntity<T>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    DateTime StatsDateTime { get; set; }
    string Note { get; set; }

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