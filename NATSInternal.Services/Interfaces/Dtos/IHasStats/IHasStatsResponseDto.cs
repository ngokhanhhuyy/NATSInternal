namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasStatsResponseDto<TBasic, TExistingAuthorization>
        : IUpsertableListResponseDto<TBasic, TExistingAuthorization>
    where TBasic : class, IHasStatsBasicResponseDto< TExistingAuthorization>
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto;