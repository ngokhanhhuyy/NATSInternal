namespace NATSInternal.Core.Interfaces.Services;

internal interface IListQueryService
{
    #region Methods
    IOrderedQueryable<TEntity> ApplySorting<TEntity>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, object?>> propertySelector,
            bool sortByAscending);

    IOrderedQueryable<TEntity> ThenApplySorting<TEntity>(
            IOrderedQueryable<TEntity> query,
            Expression<Func<TEntity, object?>> propertySelector,
            bool sortByAscending);

    Task<TListResponseDto> GetPagedListAsync<TEntity, TListResponseDto>(
            IQueryable<TEntity> query,
            ISortableAndPagableListRequestDto requestDto,
            Func<ICollection<TEntity>, int, TListResponseDto> responseDtoInitializer,
            CancellationToken cancellationToken = default) where TEntity : class;
    #endregion
}