namespace NATSInternal.Services;

/// <inheritdoc />
internal class StatsInternalService<T, TUpdateHistory>
    :
        StatsService,
        IStatsInternalService<T, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    public StatsInternalService(DatabaseContext context) : base(context) { }

    /// <inheritdoc />
    public void ValidateStatsDateTime(T entity, DateTime statsDateTime)
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
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.RetailGrossRevenue += value;
        dailyStats.Monthly.RetailGrossRevenue += value;
        await _context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task IncrementTreatmentGrossRevenueAsync(long value, DateOnly? date = null)
    {
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.TreatmentGrossRevenue += value;
        dailyStats.Monthly.TreatmentGrossRevenue += value;
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementConsultantGrossRevenueAsync(long value, DateOnly? date = null)
    {
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.ConsultantGrossRevenue += value;
        dailyStats.Monthly.ConsultantGrossRevenue += value;
        await _context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task IncrementDebtIncurredAmountAsync(long value, DateOnly? date = null)
    {
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.DebtIncurredAmount += value;
        dailyStats.Monthly.DebtIncurredAmount += value;
        await _context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task IncrementDebtPaidAmountAsync(long value, DateOnly? date = null)
    {
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.DebtPaidAmount += value;
        dailyStats.Monthly.DebtPaidAmount += value;
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementVatCollectedAmountAsync(long amount, DateOnly? date = null)
    {
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.VatCollectedAmount += amount;
        dailyStats.Monthly.VatCollectedAmount += amount;
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementShipmentCostAsync(long value, DateOnly? date = null)
    {
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.ShipmentCost += value;
        dailyStats.Monthly.ShipmentCost += value;
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementSupplyCostAsync(long value, DateOnly? date = null)
    {
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.SupplyCost += value;
        dailyStats.Monthly.SupplyCost += value;
        await _context.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task IncrementExpenseAsync(
            long value, 
            ExpenseCategory category,
            DateOnly? date = null)
    {
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
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task TemporarilyCloseAsync(DateOnly date)
    {
        DailyStats dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.TemporarilyClosedDateTime = DateTime.UtcNow.ToApplicationTime();
        await _context.SaveChangesAsync();
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
        DateOnly dateValue = date ?? DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        DailyStats dailyStats = await _context.DailyStats
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
            _context.DailyStats.Add(dailyStats);

            MonthlyStats monthlyStats = await _context.MonthlyStats
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
                _context.MonthlyStats.Add(monthlyStats);
            }

            dailyStats.Monthly = monthlyStats;
            await _context.SaveChangesAsync();
        }

        return dailyStats;
    }
}