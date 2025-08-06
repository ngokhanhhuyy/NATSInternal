namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpsertableListResponseDto<TBasic, TExistingAuthorization>
        : IListResponseDto<TBasic>
    where TBasic : class, IUpsertableBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto;