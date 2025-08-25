namespace NATSInternal.Core.Services;

internal class ListQueryService : IListQueryService
{
    #region Methods
    public async Task<TListResponseDto> GetPagedListAsync<TEntity, TListResponseDto, TBasicResponseDto>(
        IQueryable<TEntity> query,
        ISortableAndPageableListRequestDto requestDto,
        Func<TEntity, TBasicResponseDto> basicDtoInitializer,
        Func<ICollection<TBasicResponseDto>, int, TListResponseDto> listResponseDtoInitializer,
        CancellationToken cancellationToken = default)
        where TEntity : class
        where TListResponseDto : class, IPageableListResponseDto<TBasicResponseDto>
        where TBasicResponseDto : class, IBasicResponseDto
    {
        int resultCount = await query.CountAsync(cancellationToken);
        if (resultCount == 0)
        {
            return listResponseDtoInitializer(new List<TBasicResponseDto>(), 0);
        }

        int page = requestDto.Page ?? 1;
        int resultsPerPage = requestDto.ResultsPerPage ?? 15;
        int pageCount = (int)Math.Ceiling((double)resultCount / resultsPerPage);
        ICollection<TEntity> entities = await query
            .Skip(resultsPerPage * (page - 1))
            .Take(resultsPerPage)
            .ToListAsync(cancellationToken);

        return listResponseDtoInitializer(entities.Select(basicDtoInitializer).ToList(), pageCount);
    }
    #endregion
}