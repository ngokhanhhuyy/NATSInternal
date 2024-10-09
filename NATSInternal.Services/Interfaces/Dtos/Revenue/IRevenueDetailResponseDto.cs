namespace NATSInternal.Services.Interfaces.Dtos;

/// <summary>
/// A DTO containing the details of an entity which represents an transaction that
/// can generate revenue.
/// </summary>
/// <typeparam name="T">
/// The type of the DTO.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history DTOs, contained by the DTO.
/// </typeparam>
/// <typeparam name="TAuthorization">
/// The type of the authorization DTO, contained by the DTO.
/// </typeparam>
/// <inheritdoc />
internal interface IRevenueDetailResponseDto<TUpdateHistory, TAuthorization>
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