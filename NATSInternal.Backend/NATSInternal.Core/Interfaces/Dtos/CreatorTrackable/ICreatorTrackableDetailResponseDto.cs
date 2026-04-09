namespace NATSInternal.Core.Interfaces.Dtos;

internal interface ICreatorTrackableDetailResponseDto<out TAuthorization> : IUpsertableDetailResponseDto<TAuthorization>
    where TAuthorization : IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    UserBasicResponseDto CreatedUser { get; }
    #endregion
}