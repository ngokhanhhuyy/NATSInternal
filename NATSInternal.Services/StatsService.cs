namespace NATSInternal.Services;

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
        IQueryable<MonthlyStats> query = InitializeMonthlyStatsQuery(context, requestDto);

        return await query
           .Select(ms => new MonthlyStatsBasicResponseDto(ms))
           .SingleOrDefaultAsync()
           ?? throw new ResourceNotFoundException();
    }

    /// <inheritdoc />
    public async Task<MonthlyStatsDetailResponseDto> GetMonthlyDetailAsync(
            MonthlyStatsRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        IQueryable<MonthlyStats> query = InitializeMonthlyStatsQuery(context, requestDto);

        return await query
            .Include(ms => ms.DailyStats)
            .Select(ms => new MonthlyStatsDetailResponseDto(ms))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();
    }

    /// <inheritdoc />
    public async Task<DailyStatsDetailResponseDto> GetDailyDetailAsync(DateOnly? recordedDate)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateOnly date = recordedDate ?? DateOnly.FromDateTime(currentDateTime);
        return await context.DailyStats
            .Where(d => d.RecordedDate == date)
            .Select(d => new DailyStatsDetailResponseDto(d))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(DailyStats),
                nameof(date),
                date.ToVietnameseString());
    }

    /// <inheritdoc />
    public async Task<List<MonthlyStatsBasicResponseDto>> GetLastestMonthlyAsync(
            LastestMonthlyStatsRequestDto requestDto)
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
            LastestDailyStatsRequestDto requestDto)
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
            LastestDailyStatsRequestDto requestDto)
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

            return new DailyStatsDetailResponseDto(stats);
        }).ToList();
    }

    public async Task<List<ProductBasicResponseDto>> GetTopSellingProductsAsync(
            TopSellingProductsRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        IQueryable<Product> query = context.Products;
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateTime maxDateTime;
        DateTime minDateTime;

        if (requestDto.RangeType == nameof(StatsRangeType.Date))
        {
            maxDateTime = currentDateTime.Date;
            if (!requestDto.IncludeTodayOrThisMonth)
            {
                maxDateTime = maxDateTime.AddDays(-1);
            }

            minDateTime = maxDateTime.AddDays(-requestDto.RangeCount);
        }
        else
        {
            maxDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, 1);
            if (!requestDto.IncludeTodayOrThisMonth)
            {
                maxDateTime = maxDateTime.AddMonths(-1);
            }

            minDateTime = maxDateTime.AddMonths(-requestDto.RangeCount);
        }

        query = query
            .Include(p => p.OrderItems.Where(oi =>
                oi.Order.StatsDateTime >= minDateTime &&
                oi.Order.StatsDateTime < maxDateTime))
            .Include(p => p.TreatmentItems.Where(oi =>
                oi.Treatment.StatsDateTime >= minDateTime &&
                oi.Treatment.StatsDateTime < maxDateTime));

        // Determine minimum date.
        DateOnly minDate = maxDate.

        switch (requestDto.RangeType)
        {
            case nameof(StatsRangeType.Date):
                query = query
                    .Include(product => product.OrderItems)
        }
    }

    /// <summary>
    /// Initialize a query instance for monthly stats retriving operation based on the month
    /// and year specified in the request DTO.
    /// </summary>
    /// <param name="context">
    /// An instance of the <see cref="DatabaseContext"/>
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the specified month and year.
    /// </param>
    /// <returns>
    /// The query instance for the operation.
    /// </returns>
    private static IQueryable<MonthlyStats> InitializeMonthlyStatsQuery(
            DatabaseContext context,
            MonthlyStatsRequestDto requestDto)
    {
        int recordedMonth;
        int recordedYear;
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());
        if (requestDto == null)
        {
            recordedMonth = today.Month;
            recordedYear = today.Year;
        }
        else
        {
            recordedMonth = requestDto.RecordedMonth == 0
                ? today.Month
                : requestDto.RecordedMonth;
            recordedYear = requestDto.RecordedYear == 0
                ? today.Year
                : requestDto.RecordedYear;
        }

        return context.MonthlyStats
            .Where(ms => ms.RecordedYear == recordedYear && ms.RecordedMonth == recordedMonth);
    }
}