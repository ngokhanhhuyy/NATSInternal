namespace NATSInternal.Services;

/// <inheritdoc cref="IStatsInternalService" />
internal class StatsInternalService : StatsService,  IStatsInternalService
{
    public StatsInternalService(IDbContextFactory<DatabaseContext> contextFactory)
            : base(contextFactory)
    {  
    }

    /// <inheritdoc />
    public void ValidateStatsDateTime<T, TUpdateHistory>(T entity, DateTime statsDateTime)
        where T : class, IHasStatsEntity<T, TUpdateHistory>, new()
        where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    {
        string errorMessage;
        if (statsDateTime > entity.CreatedDateTime)
        {
            errorMessage = ErrorMessages.EarlierThanOrEqual
                .ReplaceComparisonValue(entity.CreatedDateTime.ToVietnameseString());
            throw new ArgumentException(errorMessage);
        }
        
        DateTime minimumAssignableDateTime;
        minimumAssignableDateTime = new DateTime(
            entity.CreatedDateTime.AddMonths(-1).Year,
            entity.CreatedDateTime.AddMonths(-1).Month,
            1, 0, 0, 0);
        if (statsDateTime < minimumAssignableDateTime)
        {
            errorMessage = ErrorMessages.GreaterThanOrEqual
                .ReplaceComparisonValue(minimumAssignableDateTime.ToVietnameseString());
            throw new ValidationException(errorMessage);
        }
    }

    /// <inheritdoc />
    public async Task IncrementRetailGrossRevenueAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.RetailGrossRevenue += value;
        dailyStats.Monthly.RetailGrossRevenue += value;
        await context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task IncrementTreatmentGrossRevenueAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.TreatmentGrossRevenue += value;
        dailyStats.Monthly.TreatmentGrossRevenue += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementConsultantGrossRevenueAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.ConsultantGrossRevenue += value;
        dailyStats.Monthly.ConsultantGrossRevenue += value;
        await context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task IncrementDebtIncurredAmountAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.DebtIncurredAmount += value;
        dailyStats.Monthly.DebtIncurredAmount += value;
        await context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task IncrementDebtPaidAmountAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.DebtPaidAmount += value;
        dailyStats.Monthly.DebtPaidAmount += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementVatCollectedAmountAsync(long amount, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.VatCollectedAmount += amount;
        dailyStats.Monthly.VatCollectedAmount += amount;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementShipmentCostAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.ShipmentCost += value;
        dailyStats.Monthly.ShipmentCost += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementSupplyCostAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.SupplyCost += value;
        dailyStats.Monthly.SupplyCost += value;
        await context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task IncrementExpenseAsync(
            long value, 
            ExpenseCategory category,
            DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        switch (category)
        {
            case ExpenseCategory.Equipment:
                dailyStats.EquipmentExpenses += value;
                dailyStats.Monthly.EquipmentExpenses += value;
                break;
            case ExpenseCategory.Office:
                dailyStats.OfficeExpense += value;
                dailyStats.Monthly.OfficeExpense += value;
                break;
            case ExpenseCategory.Utilities:
                dailyStats.UtilitiesExpenses += value;
                dailyStats.Monthly.UtilitiesExpenses += value;
                break;
            case ExpenseCategory.Staff:
                dailyStats.StaffExpense += value;
                dailyStats.Monthly.StaffExpense += value;
                break;
            default:
                string message = $"\"{nameof(category)}\" argument has invalid" +
                                $"value ({category})";
                throw new ArgumentException(message);
        }
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementNewCustomerAsync(int value = 1, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.NewCustomers += value;
        dailyStats.Monthly.NewCustomers += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task TemporarilyCloseAsync(DateOnly date)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.TemporarilyClosedDateTime = DateTime.UtcNow.ToApplicationTime();
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public DateTime GetResourceMinimumOpenedDateTime()
    {
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateTime lastMonthEditableMaxDateTime = new DateTime(
            currentDateTime.Year, currentDateTime.Month, 4,
            2, 0, 0);
        if (currentDateTime < lastMonthEditableMaxDateTime)
        {
            return new DateTime(
                currentDateTime.AddMonths(-2).Year,
                currentDateTime.AddMonths(-2).Month,
                1,
                0, 0, 0);
        }

        return new DateTime(
            currentDateTime.AddMonths(-1).Year,
            currentDateTime.AddMonths(-1).Month,
            1,
            0, 0, 0);
    }

    /// <summary>
    /// Get the daily stats entity by the given recorded date. If the recorded date
    /// is not specified, the date will be today's date value.
    /// </summary>
    /// <param name="date">
    /// Optional - The date when the stats entity is recorded. If not specified,
    /// the entity will be fetched based on today's date.</param>
    /// <returns>The DailyStats entity.</returns>
    protected async Task<DailyStats> FetchStatisticsEntitiesAsync(DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        
        DateOnly dateValue = date
            ?? DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        DailyStats dailyStats = await context.DailyStats
            .Include(ds => ds.Monthly)
            .Where(ds => ds.RecordedDate == dateValue)
            .SingleOrDefaultAsync();

        if (dailyStats == null)
        {
            dailyStats = new DailyStats
            {
                RecordedDate = dateValue,
                CreatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            };
            context.DailyStats.Add(dailyStats);

            MonthlyStats monthlyStats = await context.MonthlyStats
                .Where(ms => ms.RecordedYear == dateValue.Year)
                .Where(ms => ms.RecordedMonth == dateValue.Month)
                .SingleOrDefaultAsync();
            if (monthlyStats == null)
            {
                monthlyStats = new MonthlyStats
                {
                    RecordedMonth = dateValue.Month,
                    RecordedYear = dateValue.Year
                };
                context.MonthlyStats.Add(monthlyStats);
            }

            dailyStats.Monthly = monthlyStats;
            await context.SaveChangesAsync();
        }

        return dailyStats;
    }
}