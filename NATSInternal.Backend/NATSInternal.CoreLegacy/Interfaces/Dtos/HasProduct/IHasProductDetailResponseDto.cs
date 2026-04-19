namespace NATSInternal.Core.Interfaces.Dtos;

/// <summary>
/// A DTO containing the details of the financial entities which are related to customer.
/// </summary>
/// <typeparam name="TItem">
/// The type of the item DTOs, containing the product-related engagements' information and the products' identities.
/// </typeparam>
/// <typeparam name="TUpdateHistoryData">
/// The type of the <c>UpdateHistoryData</c> containing the update histories' information and the data of the entity
/// before and after each modification.
/// </typeparam>
/// <typeparam name="TItemUpdateHistoryData">
/// The type of the <c>ItemUpdateHistoryData</c>, containing the update histories' information and the data of the items
/// entity before and after each modification.
/// </typeparam>
/// <typeparam name="TExistingAuthorization">
/// The type of the authorization DTO, containing the information of the permissions to interact with the entity.
/// </typeparam>
internal interface IHasProductDetailResponseDto<
        TItem,
        TUpdateHistoryData,
        TItemUpdateHistoryData,
        TExistingAuthorization>
    :
        IHasStatsDetailResponseDto<TUpdateHistoryData, TExistingAuthorization>,
        IHasPhotosDetailResponseDto
    where TItem : IHasProductItemResponseDto
    where TUpdateHistoryData : IHasProductUpdateHistoryResponseDto<TItemUpdateHistoryData>
    where TItemUpdateHistoryData : IHasProductItemUpdateHistoryDataDto
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    #region Properties
    /// <summary>
    /// Contains the product-related engagements' information and the products' identities
    /// </summary>
    List<TItem> Items { get; }
    #endregion
}