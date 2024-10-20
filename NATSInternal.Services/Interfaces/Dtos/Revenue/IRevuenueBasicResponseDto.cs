namespace NATSInternal.Services.Interfaces.Dtos;

/// <summary>
/// A DTO containing the basic information of an entity which represents an transaction that
/// can generate revenue.
/// </summary>
/// <typeparam name="TAuthorization">
/// The type of the authorization DTO, containing the information of the permissions to
/// interact with the revenue.
/// </typeparam>
internal interface IRevuenueBasicResponseDto<TAuthorization>
    : ICustomerEngageableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    /// <summary>
    /// Represents the date and time when the transaction is paid.
    /// </summary>
    DateTime StatsDateTime { get; }
}