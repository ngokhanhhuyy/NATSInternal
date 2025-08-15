namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class StatsService : IStatsService
{
    protected readonly IDbContextFactory<DatabaseContext> _contextFactory;

    public StatsService(IDbContextFactory<DatabaseContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <inheritdoc />
    public async Task<MonthlyStatsBasicResponseDto> GetMonthlyBasicAsync(
            MonthlyStatsRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        return await context.MonthlyStats
           .Select(ms => new MonthlyStatsBasicResponseDto(ms))
           .SingleOrDefaultAsync()
           ?? throw new NotFoundException();
    }

    /// <inheritdoc />
    public async Task<MonthlyStatsDetailResponseDto> GetMonthlyDetailAsync(
            MonthlyStatsRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        MonthlyStats monthlyStats = await context.MonthlyStats
            .Include(ms => ms.DailyStats)
            .Where(ms =>
                ms.RecordedYear == requestDto.RecordedYear &&
                ms.RecordedMonth == requestDto.RecordedMonth)
            .SingleOrDefaultAsync();

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        int daysInMonth;
        if (requestDto.RecordedYear == today.Year && requestDto.RecordedMonth == today.Month)
        {
            daysInMonth = today.Day;
        }
        else
        {
            daysInMonth = DateTime.DaysInMonth(
                requestDto.RecordedYear,
                requestDto.RecordedMonth);
        }

        if (monthlyStats != null)
        {
            List<int> recordedDays = monthlyStats.DailyStats
                .Select(ds => ds.RecordedDate.Day)
                .ToList();

            for (int day = 1; day <= daysInMonth; day++)
            {
                if (!recordedDays.Contains(day))
                {
                    monthlyStats.DailyStats.Add(new DailyStats
                    {
                        RecordedDate = new DateOnly(
                            requestDto.RecordedYear,
                            requestDto.RecordedMonth,
                            day)
                    });
                }
            }

            monthlyStats.DailyStats = monthlyStats.DailyStats
                .Where(ds => ds.RecordedDate <= today)
                .OrderBy(ds => ds.RecordedDate.Day)
                .ToList();
        }
        else
        {
            // Generate an empty stats when the stats with the sppecified date doesn't exist.
            monthlyStats = new MonthlyStats
            {
                DailyStats = new List<DailyStats>(),
                RecordedMonth = requestDto.RecordedMonth,
                RecordedYear = requestDto.RecordedYear
            };

            for (int day = 1; day <= daysInMonth; day++)
            {
                DailyStats dailyStats = new DailyStats
                {
                    RecordedDate = new DateOnly(
                        requestDto.RecordedYear,
                        requestDto.RecordedMonth,
                        day)
                };

                monthlyStats.DailyStats.Add(dailyStats);
            }
        }

        return new MonthlyStatsDetailResponseDto(monthlyStats);
    }

    /// <inheritdoc />
    public async Task<DailyStatsDetailResponseDto> GetDailyDetailAsync(DateOnly? recordedDate)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateOnly date = recordedDate ?? DateOnly.FromDateTime(currentDateTime);
        DailyStats dailyStats = await context.DailyStats
            .Where(d => d.RecordedDate == date)
            .SingleOrDefaultAsync()
            ?? new DailyStats { RecordedDate = date };

        return new DailyStatsDetailResponseDto(dailyStats);
    }

    /// <inheritdoc />
    public async Task<List<MonthlyStatsBasicResponseDto>> GetLastestMonthlyAsync(
            LatestMonthlyStatsRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DateOnly startingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (!requestDto.IncludeThisMonth)
        {
            startingDate = startingDate.AddMonths(-1);
        }

        // Generate a series of month and year from this month (or last month if the request
        // DTO indicates not to include this month), back to the past.
        List <(int Month, int Year)> monthYearSeries = new List<(int, int)>();
        DateOnly evaluatingDate = startingDate;
        for (int i = 0; i < requestDto.MonthCount; i++)
        {
            monthYearSeries.Add((evaluatingDate.Month, evaluatingDate.Year));
            evaluatingDate = evaluatingDate.AddMonths(-1);
        }
        int endingYear = monthYearSeries.Min(mys => mys.Year);

        List<MonthlyStats> monthlyStatsList = await context.MonthlyStats
            .OrderByDescending(ms => ms.RecordedYear).ThenByDescending(ms => ms.RecordedMonth)
            .Where(ms => ms.RecordedYear >= endingYear)
            .ToListAsync();

        return monthYearSeries.Select(mys =>
        {
            MonthlyStats stats = monthlyStatsList.SingleOrDefault(ms =>
                ms.RecordedMonth == mys.Month &&
                ms.RecordedYear == mys.Year);

            if (stats == null)
            {
                return null;
            }

            return new MonthlyStatsBasicResponseDto(stats);
        }).ToList();

    }

    /// <inheritdoc />
    public async Task<List<DailyStatsBasicResponseDto>> GetLastestDailyBasicAsync(
            LatestDailyStatsRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DateOnly evaluatingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (!requestDto.IncludeToday)
        {
            evaluatingDate = evaluatingDate.AddDays(-1);
        }

        // Generate a series of dates from today (or yesterday if the request DTO indicates
        // not to include today), back to the past.
        List<DateOnly> dateSeries = new List<DateOnly>();
        for (int i = 0; i < requestDto.DayCount; i++)
        {
            dateSeries.Add(evaluatingDate);
            evaluatingDate = evaluatingDate.AddDays(-1);
        }

        List<DailyStats> dailyStatsList = await context.DailyStats
            .OrderByDescending(ds => ds.RecordedDate)
            .Where(ds => dateSeries.Contains(ds.RecordedDate))
            .ToListAsync();

        return dateSeries.Select(seriesDate =>
        {
            DailyStats stats = dailyStatsList
                .SingleOrDefault(ds => ds.RecordedDate == seriesDate);

            if (stats == null)
            {
                return null;
            }

            return new DailyStatsBasicResponseDto(stats);
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<List<DailyStatsDetailResponseDto>> GetLastestDailyDetailAsync(
            LatestDailyStatsRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DateOnly evaluatingDate = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (!requestDto.IncludeToday)
        {
            evaluatingDate = evaluatingDate.AddDays(-1);
        }

        // Generate a series of dates from today (or yesterday if the request DTO indicates
        // not to include today), back to the past.
        List<DateOnly> dateSeries = new List<DateOnly>();
        for (int i = 0; i < requestDto.DayCount; i++)
        {
            dateSeries.Add(evaluatingDate);
            evaluatingDate = evaluatingDate.AddDays(-1);
        }

        List<DailyStats> dailyStatsList = await context.DailyStats
            .OrderByDescending(ds => ds.RecordedDate)
            .Where(ds => dateSeries.Contains(ds.RecordedDate))
            .ToListAsync();

        return dateSeries.Select(seriesDate =>
        {
            DailyStats stats = dailyStatsList
                .SingleOrDefault(ds => ds.RecordedDate == seriesDate)
                ?? new DailyStats { RecordedDate = seriesDate };

            return new DailyStatsDetailResponseDto(stats);
        }).ToList();
    }

    /// <inheritdoc />
    public async Task<TopSoldProductListResponseDto> GetTopSoldProductListAsync(
            TopSoldProductListRequestDto requestDto)
    {
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

        Task<List<Order>> ordersTask = orderContext.Orders
            .Include(o => o.Items).ThenInclude(oi => oi.Product)
            .Where(o =>
                o.StatsDateTime.Date >= minDateTime &&
                o.StatsDateTime.Date <= maxDateTime)
            .ToListAsync();
        Task<List<Treatment>> treatmentsTask = treatmentContext.Treatments
            .Include(o => o.Items).ThenInclude(ti => ti.Product)
            .Where(o =>
                o.StatsDateTime.Date >= minDateTime &&
                o.StatsDateTime.Date <= maxDateTime)
            .ToListAsync();

        await Task.WhenAll(ordersTask, treatmentsTask);

        List<TopSoldProductResponseDto> responseDtos = new List<TopSoldProductResponseDto>();
        
        foreach (Order order in ordersTask.Result)
        {
            foreach (OrderItem orderItem in order.Items)
            {
                TopSoldProductResponseDto responseDto = responseDtos
                    .SingleOrDefault(dto => dto.Id == orderItem.Product.Id);
                if (responseDto == null)
                {
                    responseDto = new TopSoldProductResponseDto(orderItem.Product, 0, 0);
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

        Task<List<DebtIncurrence>> debtIncurrencesTask = debtIncurrenceContext.DebtIncurrences
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

    /// <summary>
    /// Calculates the amount of an product export item, which is the total between the item's
    /// product amount before VAT per unit and the item's VAT amount per unit.
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
        return (item.ProductAmountPerUnit + item.VatAmountPerUnit) * item.Quantity;
    }
}