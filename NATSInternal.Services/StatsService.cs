namespace NATSInternal.Services;

/// <inheritdoc />
internal class StatsService : IStatsService
{
    protected readonly DatabaseContext _context;

    public StatsService(DatabaseContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<MonthlyStatsDetailResponseDto> GetMonthlyStatsDetailAsync(
            MonthlyStatsRequestDto requestDto)
    {
        IQueryable<MonthlyStats> query = _context.MonthlyStats.Include(m => m.DailyStats);
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

        return await _context.MonthlyStats
            .Include(ms => ms.DailyStats)
            .Where(ms => ms.RecordedYear == recordedYear)
            .Where(ms => ms.RecordedMonth == recordedMonth)
            .Select(ms => new MonthlyStatsDetailResponseDto(ms))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(MonthlyStats),
                nameof(DisplayNames.RecordedMonthAndYear),
                $"Tháng {recordedMonth} năm {recordedYear}");
    }

    /// <inheritdoc />
    public async Task<DailyStatsDetailResponseDto> GetDailyStatsDetailAsync(
            DateOnly? recordedDate)
    {
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        DateOnly date = recordedDate ?? DateOnly.FromDateTime(currentDateTime);
        return await _context.DailyStats
            .Where(d => d.RecordedDate == date)
            .Select(d => new DailyStatsDetailResponseDto(d))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(DailyStats),
                nameof(date),
                date.ToVietnameseString());
    }
}