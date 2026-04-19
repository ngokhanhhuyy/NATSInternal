namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpsertableBasicResponseDto<out TExistingAuthorization> : IBasicResponseDto
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    TExistingAuthorization? Authorization { get; }
    #endregion
}