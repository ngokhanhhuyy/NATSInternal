using JetBrains;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Entities;
using NATSInternal.Core.Common.Time;

namespace NATSInternal.Core.Common.Services;

[UsedImplicitly]
internal class StatsMonthYearService : IStatsMonthYearService
{
    #region Fields
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public StatsMonthYearService(IClock clock)
    {
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task<List<StatsMonthYearResponseDto>> GetStatsMonthYearSeries<TEntity>(DbSet<TEntity> dbSet)
        where TEntity : class, IHasStatsEntity
    {
        DateOnly today = _clock.Today;
        DateOnly earliestStatsDate = await dbSet
            .OrderBy(e => e.StatsDate)
            .Select(e => e.StatsDate)
            .DefaultIfEmpty(today)
            .FirstAsync();

        List<StatsMonthYearResponseDto> responseDtos = new();
        int generatingYear = earliestStatsDate.Year;
        int generatingMonth = earliestStatsDate.Month;
        bool ShouldGenerationFinish()
        {
            int currentYear = earliestStatsDate.Year;
            int currentMonth = earliestStatsDate.Month;
            return generatingYear > currentYear || (generatingYear == currentYear && generatingMonth > currentMonth);
        }

        do
        {
            StatsMonthYearResponseDto responseDto = new()
            {
                Year = generatingYear,
                Month = generatingMonth
            };

            responseDtos.Add(responseDto);

            generatingMonth += 1;
            if (generatingMonth > 12)
            {
                generatingYear += 1;
                generatingMonth = 1;
            }
        }
        while (!ShouldGenerationFinish());

        return responseDtos;
    }
    #endregion
}
