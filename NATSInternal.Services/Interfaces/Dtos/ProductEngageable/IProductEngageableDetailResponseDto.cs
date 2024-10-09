namespace NATSInternal.Services.Interfaces.Dtos;

/// <summary>
/// A DTO containing the details of the financial entities which are related to customer.
/// </summary>
/// <typeparam name="TProductItem">
/// The type of the item DTOs, containing the product-related engagements' information and the
/// products' identities.
/// </typeparam>
/// <typeparam name="TPhoto">
/// The type of the photo DTOs, containing the details of the associated photos.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the <c>UpdateHistory</c> DTOs, containing the update histories' information
/// and the data of the entity before and after each modification.
/// </typeparam>
/// <typeparam name="TAuthorization">
/// The type of the authorization DTO, containing the information of the permissions to
/// interact with the entity.
/// </typeparam>
internal interface IProductEngageableDetailResponseDto<
        TProductItem,
        TPhoto,
        TUpdateHistory,
        TAuthorization>
    :
        IFinancialEngageableDetailResponseDto<TUpdateHistory, TAuthorization>,
        IHasPhotoDetailResponseDto<TPhoto>
    where TProductItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUpdateHistory : IUpdateHistoryResponseDto
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    /// <summary>
    /// Contains the product-related engagements' information and the
    /// products' identities
    /// </summary>
    List<TProductItem> Items { get; set; }
}