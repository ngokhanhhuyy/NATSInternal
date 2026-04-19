namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpsertableListResponseDto<TBasic, TExistingAuthorization>
        : IPageableListResponseDto<TBasic>
    where TBasic : class, IUpsertableBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto;