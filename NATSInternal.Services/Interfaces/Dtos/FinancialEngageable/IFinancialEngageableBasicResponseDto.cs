namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableBasicResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    DateTime StatsDateTime { get; }
    long AmountAfterVat { get; }
    bool IsLocked { get; }
}