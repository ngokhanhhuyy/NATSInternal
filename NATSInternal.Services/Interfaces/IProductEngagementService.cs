namespace NATSInternal.Services.Interfaces;

internal interface IProductEngagementService<in T, TItem, TProduct, TPhoto, TUser, TUpdateHistory>
    where T : class, IProductEngageableEntity<T, TItem, TProduct, TPhoto, TUser, TUpdateHistory>, new()
    where TItem : class, IProductEngageableItemEntity<TItem, TProduct>, new()
    where TProduct : class, IProductEntity<TProduct>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    Task CreateItemsAsync<TItemRequestDto>(
            T entity,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType)
        where TItemRequestDto : IProductEngageableItemRequestDto;

    Task UpdateItemsAsync<TItemRequestDto>(
            T entity,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType,
            string itemDisplayName)
        where TItemRequestDto : IProductEngageableItemRequestDto;

    void DeleteItems(
        T entity,
        DbSet<TItem> itemRepository,
        ProductEngagementType engagementType);
}