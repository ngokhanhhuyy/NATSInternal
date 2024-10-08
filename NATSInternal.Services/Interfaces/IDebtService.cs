namespace NATSInternal.Services.Interfaces;

internal interface IDebtService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TUpsertRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TUpdateHistoryResponseDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    where T :
        class,
        IFinancialEngageableEntity<T, TUpdateHistory>,
        new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto : ICustomerEngageableListRequestDto
    where TListResponseDto : IUpsertableListResponseDto<
        TBasicResponseDto,
        TAuthorizationResponseDto,
        TListAuthorizationResponseDto>
    where TBasicResponseDto : IUpsertableBasicResponseDto<TAuthorizationResponseDto>
    where TDetailResponseDto : IRevenueDetailResponseDto<
        TUpdateHistoryResponseDto,
        TAuthorizationResponseDto>
    where TUpdateHistoryResponseDto : IUpdateHistoryResponseDto
    where TListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
    where TAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    Task<TListResponseDto> GetListAsync(TListRequestDto requestDto);

    Task<TDetailResponseDto> GetDetailAsync(int id);

    Task<int> CreateAsync(TUpsertRequestDto requestDto);

    Task UpdateAsync(int id, TUpsertRequestDto requestDto);

    Task DeleteAsync(int id);
}