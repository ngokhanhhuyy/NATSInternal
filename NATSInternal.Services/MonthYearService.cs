namespace NATSInternal.Services;

/// <inheritdoc />
internal sealed class MonthYearService<T, TUpdateHistory>
    : IMonthYearService<T, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    private readonly DatabaseContext _context;
    private static MonthYearResponseDto _earliestRecordedMonthYear;

    public MonthYearService(DatabaseContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<MonthYearResponseDto>> GenerateMonthYearOptions(
            Func<DatabaseContext, DbSet<T>> repositorySelector)
    {
        _earliestRecordedMonthYear ??= await repositorySelector(_context)
            .OrderBy(T.StatsDateTimeExpression)
            .Select(entity => new MonthYearResponseDto
            {
                Year = entity.StatsDateTime.Year,
                Month = entity.StatsDateTime.Month
            }).FirstOrDefaultAsync();

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        int currentYear = currentDateTime.Year;
        int currentMonth = currentDateTime.Month;
        List<MonthYearResponseDto> monthYearOptions = new List<MonthYearResponseDto>();
        if (_earliestRecordedMonthYear != null)
        {
            for (int initializingYear = _earliestRecordedMonthYear.Year;
                initializingYear <= currentYear;
                initializingYear++)
            {
                int initializingMonth = 1;
                if (initializingYear == _earliestRecordedMonthYear.Year)
                {
                    initializingMonth = _earliestRecordedMonthYear.Month;
                }
                
                while (initializingMonth <= 12)
                {
                    MonthYearResponseDto option;
                    option = new MonthYearResponseDto(
                        initializingYear,
                        initializingMonth);
                    monthYearOptions.Add(option);
                    initializingMonth++;
                    if (initializingYear == currentYear && initializingMonth > currentMonth)
                    {
                        break;
                    }
                }
            }
            monthYearOptions.Reverse();
        }
        else
        {
            monthYearOptions.Add(new MonthYearResponseDto(currentYear, currentMonth));
        }

        return monthYearOptions;
    }
}