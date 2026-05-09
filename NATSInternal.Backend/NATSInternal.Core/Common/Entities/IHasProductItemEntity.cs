using NATSInternal.Core.Features.Products;

namespace NATSInternal.Core.Common.Entities;

internal interface IHasProductItemEntity
{
    #region ForeignKeyProperties
    int Id { get; }
    int ProductId { get; set; }
    int Quantity { get; set; }
    #endregion

    #region NavigationProperties
    Product Product { get; set; }
    #endregion
}