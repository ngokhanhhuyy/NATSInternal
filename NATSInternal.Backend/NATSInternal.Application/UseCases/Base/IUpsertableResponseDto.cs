using NATSInternal.Application.Authorization;

namespace NATSInternal.Application.UseCases;

internal interface IUpsertableResponseDto<out TExistingAuthorization>
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    TExistingAuthorization? Authorization { get; }
    #endregion
}