namespace NATSInternal.Services.Interfaces.Dtos;

/// <summary>
/// A DTO containing the details of the financial entities which are related to customer.
/// </summary>
/// <typeparam name="TItem">
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
/// <typeparam name="TItemUpdateHistoryData">
/// The type of the item DTO, containing the update histories' information
/// and the data of the items entity before and after each modification.
/// </typeparam>
/// <typeparam name="TExistingAuthorization">
/// The type of the authorization DTO, containing the information of the permissions to
/// interact with the entity.
/// </typeparam>
internal interface IProductEngageableDetailResponseDto<
        TItem,
        TPhoto,
        TUpdateHistory,
        TItemUpdateHistoryData,
        TExistingAuthorization>
    :
        IFinancialEngageableDetailResponseDto<TUpdateHistory, TExistingAuthorization>,
        IHasMultiplePhotosDetailResponseDto<TPhoto>
    where TItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUpdateHistory : IProductEngageableUpdateHistoryResponseDto<TItemUpdateHistoryData>
    where TItemUpdateHistoryData : IProductEngageableItemUpdateHistoryDataDto
    where TExistingAuthorization : IFinancialEngageableExistingAuthorizationResponseDto
{
    /// <summary>
    /// Contains the product-related engagements' information and the
    /// products' identities
    /// </summary>
    List<TItem> Items { get; }
}