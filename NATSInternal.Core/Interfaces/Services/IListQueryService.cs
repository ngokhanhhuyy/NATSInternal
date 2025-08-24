namespace NATSInternal.Core.Interfaces.Services;

internal interface IListQueryService
{
    #region Methods
    Task<TListResponseDto> GetPagedListAsync<TEntity, TListResponseDto>(
            IQueryable<TEntity> query,
            ISortableAndPageableListRequestDto requestDto,
            Func<ICollection<TEntity>, int, TListResponseDto> responseDtoInitializer,
            CancellationToken cancellationToken = default) where TEntity : class;
    #endregion
}