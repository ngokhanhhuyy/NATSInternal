namespace NATSInternal.Services.Interfaces.Dtos;

/// <summary>
/// A DTO containing the details of an entity which represents an transaction that
/// can generate revenue.
/// </summary>
/// <inheritdoc />
internal interface IRevenueDetailResponseDto<
        TUpdateHistory,
        TAuthorization>
    :
        IRevuenueBasicResponseDto<TAuthorization>,
        IFinancialEngageableDetailResponseDto<TUpdateHistory, TAuthorization> 
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    /// <summary>
    /// Represents the amount before VAT of the revenue.
    /// </summary>
    long AmountBeforeVat { get; }
    
    /// <summary>
    /// Represents the VAT amount of the revenue.
    /// </summary>
    long VatAmount { get; }
}