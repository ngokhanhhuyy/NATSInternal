namespace NATSInternal.Core.Services;

internal class ListQueryService : IListQueryService
{
    #region Methods
    public IOrderedQueryable<TEntity> ApplySorting<TEntity>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, object?>> propertySelector,
            bool sortByAscending)
    {
        return sortByAscending
            ? query.OrderBy(propertySelector)
            : query.OrderByDescending(propertySelector);
    }

    public IOrderedQueryable<TEntity> ThenApplySorting<TEntity>(
            IOrderedQueryable<TEntity> query,
            Expression<Func<TEntity, object?>> propertySelector,
            bool sortByAscending)
    {
        return sortByAscending
            ? query.ThenBy(propertySelector)
            : query.ThenByDescending(propertySelector);
    }

    public async Task<TListResponseDto> GetPagedListAsync<TEntity, TListResponseDto>(
            IQueryable<TEntity> query,
            ISortableAndPagableListRequestDto requestDto,
            Func<ICollection<TEntity>, int, TListResponseDto> responseDtoInitializer,
            CancellationToken cancellationToken = default) where TEntity : class
    {
        int resultCount = await query.CountAsync(cancellationToken);
        if (resultCount == 0)
        {
            return responseDtoInitializer(new List<TEntity>(), 0);
        }

        int page = requestDto.Page ?? 1;
        int resultsPerPage = requestDto.ResultsPerPage ?? 15;
        int pageCount = (int)Math.Ceiling((double)resultCount / resultsPerPage);
        ICollection<TEntity> entities = await query
            .Skip(resultsPerPage * (page - 1))
            .Take(resultsPerPage)
            .ToListAsync(cancellationToken);

        return responseDtoInitializer(entities, pageCount);
    }
    #endregion
}