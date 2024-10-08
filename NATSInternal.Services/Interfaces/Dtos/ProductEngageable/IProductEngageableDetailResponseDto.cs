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
public interface IProductEngageableDetailResponseDto<
        TProductItem,
        TPhoto,
        TUserBasic,
        TUpdateHistory,
        TAuthorization,
        TUserAuthorization>
    :
        IFinancialEngageableDetailResponseDto<
            TUserBasic,
            TUpdateHistory,
            TAuthorization,
            TUserAuthorization>,
        IHasPhotoDetailResponseDto<TPhoto>
    where TProductItem : IProductEngageableItemResponseDto
    where TPhoto : IPhotoResponseDto
    where TUserBasic : IUserBasicResponseDto<TUserAuthorization>
    where TUpdateHistory : IUpdateHistoryResponseDto<TUserBasic, TUserAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
    where TUserAuthorization : IUpsertableAuthorizationResponseDto
{
    /// <summary>
    /// Contains the product-related engagements' information and the
    /// products' identities
    /// </summary>
    List<TProductItem> Items { get; set; }
}