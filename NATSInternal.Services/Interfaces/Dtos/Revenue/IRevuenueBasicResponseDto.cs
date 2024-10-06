namespace NATSInternal.Services.Interfaces.Dtos;

/// <summary>
/// A DTO containing the basic information of an entity which represents an transaction that
/// can generate revenue.
/// </summary>
/// <typeparam name="TCustomer">
/// The type of the customer DTO, containing the details of the customer which this DTO is
/// associated with.
/// </typeparam>
/// <typeparam name="TAuthorization">
/// The type of the authorization DTO, containing the information of the permissions to
/// interact with the revenue.
/// </typeparam>
/// <typeparam name="TCustomerAuthorization">
/// The type of the authorization DTO, containing the information of the permissions to
/// interact with the customer associated with the revenue.
/// </typeparam>
internal interface IRevuenueBasicResponseDto<TCustomer, TAuthorization, TCustomerAuthorization>
    : ICustomerEngageableBasicResponseDto<TCustomer, TAuthorization, TCustomerAuthorization>
    where TCustomer : ICustomerBasicResponseDto<TCustomerAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TCustomerAuthorization : IUpsertableAuthorizationResponseDto
{
    /// <summary>
    /// Represents the date and time when the transaction is paid.
    /// </summary>
    DateTime PaidDateTime { get; }
    
    /// <summary>
    /// Represents the amount after VAT of the revenue.
    /// </summary>
    long AmountAfterVat { get; }
}