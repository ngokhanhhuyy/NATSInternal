namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IUpsertableBasicResponseDto<TExistingAuthorization> : IBasicResponseDto
    where TExistingAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    TExistingAuthorization? Authorization { get; }
    #endregion
}