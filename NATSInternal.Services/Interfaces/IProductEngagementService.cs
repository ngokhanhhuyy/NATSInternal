namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle product-engagement-related operations.
/// </summary>
/// <typeparam name="TItem">
/// The type of the item entity, which is the connection between the
/// <see cref="IProductEngageableEntity{T, TItem, TPhoto, TUpdateHistory}"/>
/// and the <see cref="TProduct" /> entity.
/// </typeparam>
/// <typeparam name="TPhoto">
/// The type of the photo entity, which is contained by the
/// <see cref="IProductEngageableEntity{T, TItem, TPhoto, TUpdateHistory}"/> entity that
/// contains the <see cref="TItem"/> entities.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity which is associated with the
/// <see cref="IProductEngageableEntity{T, TItem, TPhoto, TUpdateHistory}"/> entity that
/// contains the <see cref="TItem"/> entities.
/// </typeparam>
internal interface IProductEngagementService<TItem, TPhoto, TUpdateHistory>
    where TItem : class, IProductEngageableItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    /// <summary>
    /// Creates new product engagement items, based on the specified items' collection, items
    /// data and engagement type.
    /// </summary>
    /// <typeparam name="TItemRequestDto">
    /// The type of the item request DTOs that contains the data for the engagement operation.
    /// </typeparam>
    /// <param name="itemEntities">
    /// A collection of item entities that act as the connection with the products.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> where <c>T</c> is <c>TItemRequestDto</c>, containing the data
    /// for the engagement operation.
    /// </param>
    /// <param name="engagementType">
    /// The type of the engagement operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task CreateItemsAsync<TItemRequestDto>(
            ICollection<TItem> itemEntities,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType)
        where TItemRequestDto : IProductEngageableItemRequestDto;

    /// <summary>
    /// Updates the existing product engagement items and creates new product engagement items
    /// (if specified), based on the specified items' collection, items data and engagement
    /// type.
    /// </summary>
    /// <typeparam name="TItemRequestDto">
    /// The type of the item request DTOs that contains the data for the engagement operation.
    /// </typeparam>
    /// <param name="itemEntities">
    /// A collection of item entities that act as the connection with the products.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> where <c>T</c> is <c>TItemRequestDto</c>, containing the data
    /// for the engagement operation.
    /// </param>
    /// <param name="engagementType">
    /// The type of the engagement operation.
    /// </param>
    /// <param name="itemDisplayName">
    /// A <see cref="string"/> value represent the display name of the item entity, used in
    /// the error message when the operation fails.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task UpdateItemsAsync<TItemRequestDto>(
            ICollection<TItem> itemEntities,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType,
            string itemDisplayName)
        where TItemRequestDto : IProductEngageableItemRequestDto;

    /// <summary>
    /// Deletes the existing product engagement items from the specified collection and
    /// repository, based on the specified engagement operation type.
    /// </summary>
    /// <param name="itemEntities">
    /// A collection of item entities that act as the connection with the products.
    /// </param>
    /// <param name="repositorySelector">
    /// A function that is used to select the <see cref="TItem"/> repository from the given
    /// <see cref="DatabaseContext"/> instance.
    /// </param>
    /// <param name="engagementType">
    /// The type of the engagement operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    void DeleteItems(
        ICollection<TItem> itemEntities,
        Func<DatabaseContext, DbSet<TItem>> repositorySelector,
        ProductEngagementType engagementType);
}