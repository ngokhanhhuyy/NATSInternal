namespace NATSInternal.Core;

/// <inheritdoc />
internal sealed class MonthYearService<T, TUpdateHistory>
    : IMonthYearService<T, TUpdateHistory>
    where T : class, IHasStatsEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    private readonly DatabaseContext _context;
    private static ListMonthYearResponseDto _earliestRecordedMonthYear;

    public MonthYearService(DatabaseContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<ListMonthYearResponseDto>> GenerateMonthYearOptions(
            Func<DatabaseContext, DbSet<T>> repositorySelector)
    {
        _earliestRecordedMonthYear ??= await repositorySelector(_context)
            .OrderBy(e => e.StatsDateTime)
            .Select(entity => new ListMonthYearResponseDto
            {
                Year = entity.StatsDateTime.Year,
                Month = entity.StatsDateTime.Month
            }).FirstOrDefaultAsync();

        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        int currentYear = currentDateTime.Year;
        int currentMonth = currentDateTime.Month;
        List<ListMonthYearResponseDto> monthYearOptions = new List<ListMonthYearResponseDto>();
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
                    ListMonthYearResponseDto option;
                    option = new ListMonthYearResponseDto(
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
            monthYearOptions.Add(new ListMonthYearResponseDto(currentYear, currentMonth));
        }

        return monthYearOptions;
    }
}