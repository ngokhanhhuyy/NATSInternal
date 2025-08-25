namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpsertableDetailResponseDto<out TExistingAuthorization>
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    DateTime CreatedDateTime { get; }
    TExistingAuthorization? AuthorizationResponseDto { get; }
    #endregion
}