namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class SummaryService : ISummaryInternalService
{
    #region Fields
    protected readonly IDbContextFactory<DatabaseContext> _contextFactory;
    #endregion

    #region Constructors
    public SummaryService(IDbContextFactory<DatabaseContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<MonthlySummaryBasicResponseDto> GetMonthlyBasicAsync(
            MonthlyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.MonthlyStats
           .Select(ms => new MonthlySummaryBasicResponseDto(ms))
           .SingleOrDefaultAsync(cancellationToken)
           ?? throw new NotFoundException();
    }

    /// <inheritdoc />
    public async Task<MonthlyStatsDetailResponseDto> GetMonthlyDetailAsync(
            MonthlyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        MonthlySummary monthlyStats = await context.MonthlyStats
            .Include(ms => ms.DailySummaries)
            .Where(ms => ms.RecordedYear == requestDto.RecordedYear && ms.RecordedMonth == requestDto.RecordedMonth)
            .SingleOrDefaultAsync(cancellationToken)
            ?? new(requestDto.RecordedYear, requestDto.RecordedMonth);

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        int daysInMonth;
        if (requestDto.RecordedYear == today.Year && requestDto.RecordedMonth == today.Month)
        {
            daysInMonth = today.Day;
        }
        else
        {
            daysInMonth = DateTime.DaysInMonth(requestDto.RecordedYear, requestDto.RecordedMonth);
        }

        return new MonthlyStatsDetailResponseDto(monthlyStats);
    }

    /// <inheritdoc />
    public async Task<DailySummaryDetailResponseDto> GetDailyDetailAsync(
            DateOnly? recordedDate,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateOnly date = recordedDate ?? DateOnly.FromDateTime(currentDateTime);
        DailySummary dailyStats = await context.DailyStats
            .Where(d => d.RecordedDate == date)
            .SingleOrDefaultAsync(cancellationToken)
            ?? new(date);

        return new DailySummaryDetailResponseDto(dailyStats);
    }

    /// <inheritdoc />
    public async Task<List<MonthlySummaryBasicResponseDto>> GetLastestMonthlyAsync(
            LatestMonthlyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DateOnly startingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (!requestDto.IncludeThisMonth)
        {
            startingDate = startingDate.AddMonths(-1);
        }

        // Generate a series of month and year from this month (or last month if the request
        // DTO indicates not to include this month), back to the past.
        List<(int Month, int Year)> monthYearSeries = new();
        DateOnly evaluatingDate = startingDate;
        for (int i = 0; i < requestDto.MonthCount; i++)
        {
            monthYearSeries.Add((evaluatingDate.Month, evaluatingDate.Year));
            evaluatingDate = evaluatingDate.AddMonths(-1);
        }

        int endingYear = monthYearSeries.Min(mys => mys.Year);

        List<MonthlySummary> monthlyStatsList = await context.MonthlyStats
            .OrderByDescending(ms => ms.RecordedYear).ThenByDescending(ms => ms.RecordedMonth)
            .Where(ms => ms.RecordedYear >= endingYear)
            .ToListAsync(cancellationToken);

        return monthYearSeries.Select(monthYear =>
        {
            MonthlySummary stats = monthlyStatsList
                .SingleOrDefault(ms => ms.RecordedYear == monthYear.Year && ms.RecordedMonth == monthYear.Month)
                ?? new(monthYear.Year, monthYear.Month);

            return new MonthlySummaryBasicResponseDto(stats);
        }).ToList();

    }

    /// <inheritdoc />
    public async Task<List<DailySummaryBasicResponseDto>> GetLastestDailyBasicAsync(
            LatestDailyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DateOnly evaluatingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (!requestDto.IncludeToday)
        {
            evaluatingDate = evaluatingDate.AddDays(-1);
        }

        // Generate a series of dates from today (or yesterday if the request DTO indicates not to include today), back
        // to the past.
        List<DateOnly> dateSeries = new List<DateOnly>();
        for (int i = 0; i < requestDto.DayCount; i++)
        {
            dateSeries.Add(evaluatingDate);
            evaluatingDate = evaluatingDate.AddDays(-1);
        }

        List<DailySummary> dailyStatsList = await context.DailyStats
            .OrderByDescending(ds => ds.RecordedDate)
            .Where(ds => dateSeries.Contains(ds.RecordedDate))
            .ToListAsync(cancellationToken);

        return dateSeries.Select(seriesDate =>
        {
            DailySummary stats = dailyStatsList
                .SingleOrDefault(ds => ds.RecordedDate == seriesDate)
                ?? new(seriesDate);

            return new DailySummaryBasicResponseDto(stats);
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<DailySummaryDetailResponseDto>> GetLastestDailyDetailAsync(
            LatestDailyStatsRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DateOnly evaluatingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (!requestDto.IncludeToday)
        {
            evaluatingDate = evaluatingDate.AddDays(-1);
        }

        // Generate a series of dates from today (or yesterday if the request DTO indicates not to include today), back
        // to the past.
        List<DateOnly> dateSeries = new List<DateOnly>();
        for (int i = 0; i < requestDto.DayCount; i++)
        {
            dateSeries.Add(evaluatingDate);
            evaluatingDate = evaluatingDate.AddDays(-1);
        }

        List<DailySummary> dailyStatsList = await context.DailyStats
            .OrderByDescending(ds => ds.RecordedDate)
            .Where(ds => dateSeries.Contains(ds.RecordedDate))
            .ToListAsync(cancellationToken);

        return dateSeries.Select(seriesDate =>
        {
            DailySummary stats = dailyStatsList
                .SingleOrDefault(ds => ds.RecordedDate == seriesDate)
                ?? new(seriesDate);

            return new DailySummaryDetailResponseDto(stats);
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<TopSoldProductListResponseDto> GetTopSoldProductListAsync(
            TopSoldProductListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext orderContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await using DatabaseContext treatmentContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateTime maxDateTime, minDateTime;

        if (requestDto.RangeType == nameof(StatsRangeType.Date))
        {
            maxDateTime = currentDateTime.Date;
            if (!requestDto.IncludeTodayOrThisMonth)
            {
                maxDateTime = maxDateTime.AddDays(-1);
            }

            minDateTime = maxDateTime.AddDays(-requestDto.RangeLength);
        }
        else
        {
            maxDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
            if (!requestDto.IncludeTodayOrThisMonth)
            {
                maxDateTime = maxDateTime.AddMonths(-1);
            }

            minDateTime = maxDateTime.AddMonths(-requestDto.RangeLength);
        }

        List<Order> orders = await orderContext.Orders
            .Include(o => o.Items).ThenInclude(oi => oi.Product)
            .Where(o => o.StatsDateTime.Date >= minDateTime && o.StatsDateTime.Date <= maxDateTime)
            .ToListAsync(cancellationToken);

        List<TopSoldProductResponseDto> responseDtos = new();
        
        foreach (Order order in orders)
        {
            foreach (OrderItem orderItem in order.Items)
            {
                Product? product = orderItem.Product;
                if (product is null)
                {
                    continue;
                }

                TopSoldProductResponseDto? responseDto = responseDtos.SingleOrDefault(dto => dto.Id == product.Id);
                if (responseDto == null)
                {
                    responseDto = new TopSoldProductResponseDto(product, 0, 0);
                    responseDtos.Add(responseDto);
                }

                responseDto.Amount +=
                    (orderItem.AmountBeforeVatPerUnit + orderItem.VatAmountPerUnit) * orderItem.Quantity;
                responseDto.Quantity += orderItem.Quantity;
            }
        }

        if (requestDto.Creteria == nameof(TopSoldProductCriteria.Amount))
        {
            responseDtos = responseDtos
                .OrderByDescending(dto => dto.Amount)
                .ThenByDescending(dto => dto.Quantity)
                .Take(requestDto.Count)
                .ToList();
        }
        else
        {
            responseDtos = responseDtos
                .OrderByDescending(dto => dto.Quantity)
                .ThenByDescending(dto => dto.Amount)
                .Take(requestDto.Count)
                .ToList();
        }

        return new TopSoldProductListResponseDto
        {
            StartingDate = DateOnly.FromDateTime(minDateTime.Date),
            EndingDate = DateOnly.FromDateTime(maxDateTime.Date),
            Items = responseDtos
        };
    }

    /// <inheritdoc />
    public async Task<TopPurchasedCustomerListResponseDto> GetTopPurchasedCustomerListAsync(
            TopPurchasedCustomerListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateTime maxDateTime, minDateTime;

        if (requestDto.RangeType == nameof(StatsRangeType.Date))
        {
            maxDateTime = currentDateTime.Date;
            if (!requestDto.IncludeTodayOrThisMonth)
            {
                maxDateTime = maxDateTime.AddDays(-1);
            }

            minDateTime = maxDateTime.AddDays(-requestDto.RangeLength);
        }
        else
        {
            maxDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
            if (!requestDto.IncludeTodayOrThisMonth)
            {
                maxDateTime = maxDateTime.AddMonths(-1);
            }

            minDateTime = maxDateTime.AddMonths(-requestDto.RangeLength);
        }

        List<Order> orders = await context.Orders
            .Include(o => o.Customer)
            .Where(o => o.StatsDateTime.Date >= minDateTime && o.StatsDateTime.Date <= maxDateTime)
            .ToListAsync(cancellationToken);

        List<TopPurchasedCustomerResponseDto> responseDtos = new();

        foreach (Order order in orders)
        {
            TopPurchasedCustomerResponseDto? responseDto = responseDtos
                .SingleOrDefault(dto => dto.Id == order.Customer.Id);

            if (responseDto is null)
            {
                responseDto = new TopPurchasedCustomerResponseDto(order.Customer);
                responseDtos.Add(responseDto);
            }

            responseDto.PurchasedAmount += order.CachedAmountAfterVat;
            responseDto.PurchasedTransactionCount += 1;
        }

        if (requestDto.Creteria == nameof(TopPurchasedCustomerCriteria.PurchasedAmount))
        {
            responseDtos = responseDtos
                .OrderByDescending(dto => dto.PurchasedAmount)
                .ThenByDescending(dto => dto.PurchasedTransactionCount)
                .Take(requestDto.Count)
                .ToList();
        }
        else
        {
            responseDtos = responseDtos
                .OrderByDescending(dto => dto.PurchasedTransactionCount)
                .ThenByDescending(dto => dto.PurchasedAmount)
                .Take(requestDto.Count)
                .ToList(); 
        }

        return new TopPurchasedCustomerListResponseDto
        {
            StartingDate = DateOnly.FromDateTime(minDateTime.Date),
            EndingDate = DateOnly.FromDateTime(maxDateTime.Date),
            Items = responseDtos
        };
    }

    /// <inheritdoc />
    public async Task<List<LastestTransactionResponseDto>> GetLatestTransactionsAsync(
            LatestTransactionsRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        //await using DatabaseContext supplyContext = await _contextFactory
        //    .CreateDbContextAsync(cancellationToken);
        //await using DatabaseContext expenseContext = await _contextFactory
        //    .CreateDbContextAsync(cancellationToken);
        //await using DatabaseContext orderContext = await _contextFactory
        //    .CreateDbContextAsync();
        //await using DatabaseContext debtContext = await _contextFactory
        //    .CreateDbContextAsync();
        //await using DatabaseContext debtPaymentContext = await _contextFactory
        //    .CreateDbContextAsync();

        //Task<List<Supply>> suppliesTask = supplyContext.Supplies
        //    .Include(supply => supply.Items)
        //    .OrderByDescending(supply => supply.StatsDateTime)
        //    .Take(requestDto.Count)
        //    .ToListAsync();

        //Task<List<Expense>> expensesTask = expenseContext.Expenses
        //    .OrderByDescending(expense => expense.StatsDateTime)
        //    .Take(requestDto.Count)
        //    .ToListAsync();

        //Task<List<Order>> ordersTask = orderContext.Orders
        //    .Include(order => order.Items)
        //    .OrderByDescending(order => order.StatsDateTime)
        //    .Take(requestDto.Count)
        //    .ToListAsync();

        //Task<List<Treatment>> treatmentsTask = treatmentContext.Treatments
        //    .Include(treatment => treatment.Items)
        //    .OrderByDescending(treatment => treatment.StatsDateTime)
        //    .Take(requestDto.Count)
        //    .ToListAsync();

        //Task<List<Debt>> debtIncurrencesTask = debtContext.DebtIncurrences
        //    .OrderByDescending(debtIncurrence => debtIncurrence.StatsDateTime)
        //    .Take(requestDto.Count)
        //    .ToListAsync();

        //Task<List<DebtPayment>> debtPaymentsTask = debtPaymentContext.DebtPayments
        //    .OrderByDescending(debtPayment => debtPayment.StatsDateTime)
        //    .Take(requestDto.Count)
        //    .ToListAsync();

        //await Task.WhenAll(
        //    suppliesTask,
        //    expensesTask,
        //    consultantsTask,
        //    ordersTask,
        //    treatmentsTask,
        //    debtIncurrencesTask,
        //    debtPaymentsTask);

        //return suppliesTask.Result
        //    .Select(s => new LastestTransactionResponseDto(s))
        //    .Union(expensesTask.Result.Select(e => new LastestTransactionResponseDto(e)))
        //    .Union(consultantsTask.Result.Select(c => new LastestTransactionResponseDto(c)))
        //    .Union(ordersTask.Result.Select(o => new LastestTransactionResponseDto(o)))
        //    .Union(treatmentsTask.Result.Select(t => new LastestTransactionResponseDto(t)))
        //    .Union(debtIncurrencesTask.Result
        //        .Select(di => new LastestTransactionResponseDto(di)))
        //    .Union(debtPaymentsTask.Result.Select(dp => new LastestTransactionResponseDto(dp)))
        //    .OrderByDescending(dto => dto.StatsDateTime)
        //    .Take(requestDto.Count)
        //    .ToList();

        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Supplies
                .OrderByDescending(s => s.StatsDateTime)
                .Select(s => new LastestTransactionResponseDto(s))
                .Take(requestDto.Count)
            .Concat(context.Expenses
                .OrderByDescending(e => e.StatsDateTime)
                .Select(e => new LastestTransactionResponseDto(e))
                .Take(requestDto.Count))
            .Concat(context.Orders
                .OrderByDescending(o => o.StatsDateTime)
                .Select(o => new LastestTransactionResponseDto(o))
                .Take(requestDto.Count))
            .Concat(context.Debts
                .OrderByDescending(d => d.StatsDateTime)
                .Select(d => new LastestTransactionResponseDto(d))
                .Take(requestDto.Count))
            .OrderByDescending(s => s.StatsDateTime)
            .Take(requestDto.Count)
            .ToListAsync(cancellationToken);

    }

    /// <inheritdoc />
    public StatsRangeTypeOptionListResponseDto GetTopSoldProductRangeTypeOptions()
    {
        return new StatsRangeTypeOptionListResponseDto(StatsRangeType.Date);
    }

    /// <inheritdoc />
    public StatsCriteriaOptionListResponseDto GetTopSoldProductCriteriaOptions()
    {
        return new StatsCriteriaOptionListResponseDto(
            typeof(TopSoldProductCriteria),
            TopSoldProductCriteria.Amount);
    }
    
    /// <inheritdoc />
    public StatsRangeTypeOptionListResponseDto GetTopPuschasedCustomerRangeTypeOptions()
    {
        return new StatsRangeTypeOptionListResponseDto(StatsRangeType.Date);
    }

    /// <inheritdoc />
    public StatsCriteriaOptionListResponseDto GetTopPurchasedCustomerCriteriaOptions()
    {
        return new StatsCriteriaOptionListResponseDto(
            typeof(TopPurchasedCustomerCriteria),
            TopPurchasedCustomerCriteria.PurchasedAmount);
    }

    /// <inheritdoc />
    public async Task<List<DateOnly>> GetSummaryDateOptionsAsync(CancellationToken cancellationToken = default)
    {
        DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DateOnly? startingDate = await context.DailyStats
            .OrderBy(ds => ds.RecordedDate)
            .Select(ds => (DateOnly?)ds.RecordedDate)
            .FirstOrDefaultAsync(cancellationToken);

        List<DateOnly> dateOptions = new();
        if (startingDate.HasValue)
        {
            DateOnly endingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
            DateOnly evaluatingDate = startingDate.Value;
            while (evaluatingDate <= endingDate)
            {
                dateOptions.Add(evaluatingDate);
                evaluatingDate = evaluatingDate.AddDays(1);
            }
        }

        return dateOptions;
    }

    /// <inheritdoc />
    public bool IsStatsDateTimeValid<TEntity, TData>(TEntity entity, DateTime statsDateTime, out string errorMessage)
        where TEntity : class, IHasStatsEntity<TEntity, TData>
        where TData : class
    {
        errorMessage = string.Empty;
        if (statsDateTime > entity.CreatedDateTime)
        {
            string errorMessageDateTime = entity.CreatedDateTime.ToVietnameseString();
            errorMessage = ErrorMessages.EarlierThanOrEqual.ReplaceComparisonValue(errorMessageDateTime);

            return false;
        }

        DateOnly minimumAssignableDate = DateOnly.FromDateTime(entity.CreatedDateTime).AddMonths(-1);
        DateTime minimumAssignableDateTime = new(minimumAssignableDate.Year, minimumAssignableDate.Month, 1, 0, 0, 0);
        if (statsDateTime < minimumAssignableDateTime)
        {
            string errorMessageDateTime = minimumAssignableDateTime.ToVietnameseString();
            errorMessage = ErrorMessages.GreaterThanOrEqual.ReplaceComparisonValue(errorMessageDateTime);

            return false;
        }

        return true;
    }

    /// <inheritdoc />
    public async Task IncrementRetailGrossRevenueAsync(
            long value,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.RetailGrossRevenue += value;
        dailyStats.MonthlySummary.RetailGrossRevenue += value;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementTreatmentGrossRevenueAsync(
            long value,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.TreatmentGrossRevenue += value;
        dailyStats.MonthlySummary.TreatmentGrossRevenue += value;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementConsultantGrossRevenueAsync(
            long value,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.ConsultantGrossRevenue += value;
        dailyStats.MonthlySummary.ConsultantGrossRevenue += value;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementDebtIncurredAmountAsync(
            long value,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.DebtIncurredAmount += value;
        dailyStats.MonthlySummary.DebtIncurredAmount += value;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementDebtPaidAmountAsync(
            long value,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.DebtPaidAmount += value;
        dailyStats.MonthlySummary.DebtPaidAmount += value;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementVatCollectedAmountAsync(
            long amount,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.VatCollectedAmount += amount;
        dailyStats.MonthlySummary.VatCollectedAmount += amount;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementShipmentCostAsync(
            long value,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.ShipmentCost += value;
        dailyStats.MonthlySummary.ShipmentCost += value;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementSupplyCostAsync(
            long value,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.SupplyCost += value;
        dailyStats.MonthlySummary.SupplyCost += value;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementExpenseAsync(
            long value,
            ExpenseCategory category,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        switch (category)
        {
            case ExpenseCategory.Equipment:
                dailyStats.EquipmentExpenses += value;
                dailyStats.MonthlySummary.EquipmentExpenses += value;
                break;
            case ExpenseCategory.Office:
                dailyStats.OfficeExpense += value;
                dailyStats.MonthlySummary.OfficeExpense += value;
                break;
            case ExpenseCategory.Utilities:
                dailyStats.UtilitiesExpenses += value;
                dailyStats.MonthlySummary.UtilitiesExpenses += value;
                break;
            case ExpenseCategory.Staff:
                dailyStats.StaffExpense += value;
                dailyStats.MonthlySummary.StaffExpense += value;
                break;
            default:
                throw new NotImplementedException();
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task IncrementNewCustomerCountAsync(
            int value = 1,
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.NewCustomerCount += value;
        dailyStats.MonthlySummary.NewCustomerCount += value;
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task TemporarilyCloseAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        DailySummary dailyStats = await FetchDailySummaryAsync(date, cancellationToken);
        dailyStats.TemporarilyClosedDateTime = DateTime.UtcNow.ToApplicationTime();
        await context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public DateTime GetResourceMinimumOpenedDateTime()
    {
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateTime lastMonthEditableMaxDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, 4, 2, 0, 0);
        if (currentDateTime < lastMonthEditableMaxDateTime)
        {
            return new DateTime(currentDateTime.AddMonths(-2).Year, currentDateTime.AddMonths(-2).Month, 1, 0, 0, 0);
        }

        return new DateTime(currentDateTime.AddMonths(-1).Year, currentDateTime.AddMonths(-1).Month, 1, 0, 0, 0);
    }
    #endregion

    #region ProtectedMethods
    /// <summary>
    /// Get the daily summary entity by the given recorded date. If the recorded date is not specified, the date will be
    /// today's date value.
    /// </summary>
    /// <param name="date">
    /// (Optional) The date when the stats entity is recorded. If not specified, the entity will be fetched based on
    /// today's date.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// The DailyStats entity.
    /// </returns>
    protected async Task<DailySummary> FetchDailySummaryAsync(
            DateOnly? date = null,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        DateOnly dateValue = date ?? DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        DailySummary? dailySummary = await context.DailyStats
            .Include(ds => ds.MonthlySummary)
            .Where(ds => ds.RecordedDate == dateValue)
            .SingleOrDefaultAsync(cancellationToken);

        if (dailySummary == null)
        {
            dailySummary = new DailySummary(dateValue);
            context.DailyStats.Add(dailySummary);

            MonthlySummary? monthlySummary = await context.MonthlyStats
                .Where(ms => ms.RecordedYear == dateValue.Year && ms.RecordedMonth == dateValue.Month)
                .SingleOrDefaultAsync(cancellationToken);

            if (monthlySummary == null)
            {
                monthlySummary = new(dateValue);
                context.MonthlyStats.Add(monthlySummary);
            }

            monthlySummary.DailySummaries.Add(dailySummary);
            await context.SaveChangesAsync(cancellationToken);
        }

        return dailySummary;
    }
    #endregion
}