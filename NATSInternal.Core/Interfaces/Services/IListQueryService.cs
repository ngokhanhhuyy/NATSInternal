namespace NATSInternal.Core.Interfaces.Services;

internal interface IListQueryService
{
    #region Methods
    Task<TListResponseDto> GetPagedListAsync<TEntity, TListResponseDto, TBasicResponseDto>(
        IQueryable<TEntity> query,
        ISortableAndPageableListRequestDto requestDto,
        Func<TEntity, TBasicResponseDto> basicDtoInitializer,
        Func<ICollection<TBasicResponseDto>, int, TListResponseDto> listResponseDtoInitializer,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TListResponseDto : class, IPageableListResponseDto<TBasicResponseDto>
        where TBasicResponseDto : class, IBasicResponseDto;
    #endregion
}