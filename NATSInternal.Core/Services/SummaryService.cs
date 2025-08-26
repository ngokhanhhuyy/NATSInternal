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
    public async Task<DailySummaryDetailResponseDto> GetDailyDetailAsync(DateOnly? recordedDate)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateOnly date = recordedDate ?? DateOnly.FromDateTime(currentDateTime);
        DailySummary dailyStats = await context.DailyStats
            .Where(d => d.RecordedDate == date)
            .SingleOrDefaultAsync()
            ?? new DailySummary(date);

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

                responseDto.Amount += CalculateExportProductItemAmount(orderItem);
                responseDto.Quantity += orderItem.Quantity;
            }
        }

        foreach (Treatment treatment in treatmentsTask.Result)
        {
            foreach (TreatmentItem treatmentItem in treatment.Items)
            {
                TopSoldProductResponseDto responseDto = responseDtos
                    .SingleOrDefault(dto => dto.Id == treatmentItem.Product.Id);
                if (responseDto == null)
                {
                    responseDto = new TopSoldProductResponseDto(treatmentItem.Product, 0, 0);
                    responseDtos.Add(responseDto);
                }
                
                responseDto.Amount += CalculateExportProductItemAmount(treatmentItem);
                responseDto.Quantity += treatmentItem.Quantity;
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
            TopPurchasedCustomerListRequestDto requestDto)
    {
        await using DatabaseContext consultantContext = await _contextFactory
            .CreateDbContextAsync();
        await using DatabaseContext orderContext = await _contextFactory
            .CreateDbContextAsync();
        await using DatabaseContext treatmentContext = await _contextFactory
            .CreateDbContextAsync();

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

        Task<List<Consultant>> consultantsTask = consultantContext.Consultants
            .Include(c => c.Customer)
            .Where(c =>
                c.StatsDateTime.Date >= minDateTime &&
                c.StatsDateTime.Date <= maxDateTime)
            .ToListAsync();
        Task<List<Order>> ordersTask = orderContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
            .Where(o =>
                o.StatsDateTime.Date >= minDateTime &&
                o.StatsDateTime.Date <= maxDateTime)
            .ToListAsync();
        Task<List<Treatment>> treatmentsTask = treatmentContext.Treatments
            .Include(t => t.Customer)
            .Include(t => t.Items)
            .Where(o =>
                o.StatsDateTime.Date >= minDateTime &&
                o.StatsDateTime.Date <= maxDateTime)
            .ToListAsync();

        await Task.WhenAll(consultantsTask, ordersTask, treatmentsTask);

        List<TopPurchasedCustomerResponseDto> responseDtos;
        responseDtos = new List<TopPurchasedCustomerResponseDto>();

        foreach (Consultant consultant in consultantsTask.Result)
        {
            TopPurchasedCustomerResponseDto responseDto = responseDtos
                .SingleOrDefault(dto => dto.Id == consultant.Customer.Id);

            if (responseDto == null)
            {
                responseDto = new TopPurchasedCustomerResponseDto(consultant.Customer);
                responseDtos.Add(responseDto);
            }

            responseDto.PurchasedAmount += consultant.AmountBeforeVat + consultant.VatAmount;
            responseDto.PurchasedTransactionCount += 1;
        }

        foreach (Order order in ordersTask.Result)
        {
            TopPurchasedCustomerResponseDto responseDto = responseDtos
                .SingleOrDefault(dto => dto.Id == order.Customer.Id);

            if (responseDto == null)
            {
                responseDto = new TopPurchasedCustomerResponseDto(order.Customer);
                responseDtos.Add(responseDto);
            }

            responseDto.PurchasedAmount += order.AmountAfterVat;
            responseDto.PurchasedTransactionCount += 1;
        }

        foreach (Treatment treatment in treatmentsTask.Result)
        {
            TopPurchasedCustomerResponseDto responseDto = responseDtos
                .SingleOrDefault(dto => dto.Id == treatment.Customer.Id);

            if (responseDto == null)
            {
                responseDto = new TopPurchasedCustomerResponseDto(treatment.Customer);
                responseDtos.Add(responseDto);
            }

            responseDto.PurchasedAmount += treatment.AmountAfterVat;
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
            LatestTransactionsRequestDto requestDto)
    {
        await using DatabaseContext supplyContext = await _contextFactory
            .CreateDbContextAsync();
        await using DatabaseContext expenseContext = await _contextFactory
            .CreateDbContextAsync();
        await using DatabaseContext consultantContext = await _contextFactory
            .CreateDbContextAsync();
        await using DatabaseContext orderContext = await _contextFactory
            .CreateDbContextAsync();
        await using DatabaseContext treatmentContext = await _contextFactory
            .CreateDbContextAsync();
        await using DatabaseContext debtIncurrenceContext = await _contextFactory
            .CreateDbContextAsync();
        await using DatabaseContext debtPaymentContext = await _contextFactory
            .CreateDbContextAsync();

        Task<List<Supply>> suppliesTask = supplyContext.Supplies
            .Include(supply => supply.Items)
            .OrderByDescending(supply => supply.StatsDateTime)
            .Take(requestDto.Count)
            .ToListAsync();

        Task<List<Expense>> expensesTask = expenseContext.Expenses
            .OrderByDescending(expense => expense.StatsDateTime)
            .Take(requestDto.Count)
            .ToListAsync();

        Task<List<Consultant>> consultantsTask = consultantContext.Consultants
            .OrderByDescending(consultant => consultant.StatsDateTime)
            .Take(requestDto.Count)
            .ToListAsync();

        Task<List<Order>> ordersTask = orderContext.Orders
            .Include(order => order.Items)
            .OrderByDescending(order => order.StatsDateTime)
            .Take(requestDto.Count)
            .ToListAsync();

        Task<List<Treatment>> treatmentsTask = treatmentContext.Treatments
            .Include(treatment => treatment.Items)
            .OrderByDescending(treatment => treatment.StatsDateTime)
            .Take(requestDto.Count)
            .ToListAsync();

        Task<List<Debt>> debtIncurrencesTask = debtIncurrenceContext.DebtIncurrences
            .OrderByDescending(debtIncurrence => debtIncurrence.StatsDateTime)
            .Take(requestDto.Count)
            .ToListAsync();

        Task<List<DebtPayment>> debtPaymentsTask = debtPaymentContext.DebtPayments
            .OrderByDescending(debtPayment => debtPayment.StatsDateTime)
            .Take(requestDto.Count)
            .ToListAsync();

        await Task.WhenAll(
            suppliesTask,
            expensesTask,
            consultantsTask,
            ordersTask,
            treatmentsTask,
            debtIncurrencesTask,
            debtPaymentsTask);

        return suppliesTask.Result
            .Select(s => new LastestTransactionResponseDto(s))
            .Union(expensesTask.Result.Select(e => new LastestTransactionResponseDto(e)))
            .Union(consultantsTask.Result.Select(c => new LastestTransactionResponseDto(c)))
            .Union(ordersTask.Result.Select(o => new LastestTransactionResponseDto(o)))
            .Union(treatmentsTask.Result.Select(t => new LastestTransactionResponseDto(t)))
            .Union(debtIncurrencesTask.Result
                .Select(di => new LastestTransactionResponseDto(di)))
            .Union(debtPaymentsTask.Result.Select(dp => new LastestTransactionResponseDto(dp)))
            .OrderByDescending(dto => dto.StatsDateTime)
            .Take(requestDto.Count)
            .ToList();
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
    public async Task<List<DateOnly>> GetStatsDateOptionsAsync()
    {
        DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DateOnly? startingDate = await context.DailyStats
            .OrderBy(ds => ds.RecordedDate)
            .Select(ds => (DateOnly?)ds.RecordedDate)
            .FirstOrDefaultAsync();

        List<DateOnly> dateOptions = new List<DateOnly>();
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
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.RetailGrossRevenue += value;
        dailyStats.MonthlySummary.RetailGrossRevenue += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementTreatmentGrossRevenueAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.TreatmentGrossRevenue += value;
        dailyStats.MonthlySummary.TreatmentGrossRevenue += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementConsultantGrossRevenueAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.ConsultantGrossRevenue += value;
        dailyStats.MonthlySummary.ConsultantGrossRevenue += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementDebtIncurredAmountAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.DebtIncurredAmount += value;
        dailyStats.MonthlySummary.DebtIncurredAmount += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementDebtPaidAmountAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.DebtPaidAmount += value;
        dailyStats.MonthlySummary.DebtPaidAmount += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementVatCollectedAmountAsync(long amount, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.VatCollectedAmount += amount;
        dailyStats.MonthlySummary.VatCollectedAmount += amount;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementShipmentCostAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.ShipmentCost += value;
        dailyStats.MonthlySummary.ShipmentCost += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementSupplyCostAsync(long value, DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.SupplyCost += value;
        dailyStats.MonthlySummary.SupplyCost += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task IncrementExpenseAsync(
            long value,
            ExpenseCategory category,
            DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
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
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
        dailyStats.NewCustomerCount += value;
        dailyStats.MonthlySummary.NewCustomerCount += value;
        await context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task TemporarilyCloseAsync(DateOnly date)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DailySummary dailyStats = await FetchStatisticsEntitiesAsync(date);
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
    #endregion

    #region StaticMethods

    /// <summary>
    /// Get the daily stats entity by the given recorded date. If the recorded date
    /// is not specified, the date will be today's date value.
    /// </summary>
    /// <param name="date">
    /// Optional - The date when the stats entity is recorded. If not specified,
    /// the entity will be fetched based on today's date.</param>
    /// <returns>The DailyStats entity.</returns>
    protected async Task<DailySummary> FetchStatisticsEntitiesAsync(DateOnly? date = null)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();

        DateOnly dateValue = date
            ?? DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        DailySummary dailyStats = await context.DailyStats
            .Include(ds => ds.MonthlySummary)
            .Where(ds => ds.RecordedDate == dateValue)
            .SingleOrDefaultAsync();

        if (dailyStats == null)
        {
            dailyStats = new DailySummary
            {
                RecordedDate = dateValue,
                CreatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            };
            context.DailyStats.Add(dailyStats);

            MonthlySummary monthlyStats = await context.MonthlyStats
                .Where(ms => ms.RecordedYear == dateValue.Year)
                .Where(ms => ms.RecordedMonth == dateValue.Month)
                .SingleOrDefaultAsync();
            if (monthlyStats == null)
            {
                monthlyStats = new MonthlySummary
                {
                    RecordedMonth = dateValue.Month,
                    RecordedYear = dateValue.Year
                };
                context.MonthlyStats.Add(monthlyStats);
            }

            dailyStats.MonthlySummary = monthlyStats;
            await context.SaveChangesAsync();
        }

        return dailyStats;
    }

    /// <summary>
    /// Calculates the amount of an product export item, which is the total between the item's product amount before VAT
    /// per unit and the item's VAT amount per unit.
    /// </summary>
    /// <typeparam name="TItemEntity">
    /// The type of the item entity.
    /// </typeparam>
    /// <param name="item">
    /// An instance of the item entity.
    /// </param>
    /// <returns>
    /// The total amount of the item.
    /// </returns>
    private static long CalculateExportProductItemAmount<TItemEntity>(TItemEntity item)
        where TItemEntity : class, IExportProductItemEntity<TItemEntity>, new()
    {
        return (item.AmountPerUnit + item.VatAmountPerUnit) * item.Quantity;
    }
    #endregion
}