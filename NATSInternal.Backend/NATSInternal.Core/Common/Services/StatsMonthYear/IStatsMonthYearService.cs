using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Entities;

namespace NATSInternal.Core.Common.Services;

internal interface IStatsMonthYearService
{
    #region Methods
    Task<List<StatsMonthYearResponseDto>> GetStatsMonthYearSeries<TEntity>(DbSet<TEntity> dbSet)
        where TEntity : class, IHasStatsEntity;
    #endregion
}
