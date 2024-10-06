namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductEntity<T> : IUpsertableEntity<T> where T : class, new()
{ 
    string Name { get; set; }
    string Description { get; set; }
    string Unit { get; set; }
    long DefaultPrice { get; set; }
    int DefaultVatPercentage { get; set; }
    bool IsForRetail { get; set; }
    bool IsDiscontinued { get; set; }
    DateTime? UpdatedDateTime { get; set; }
    string ThumbnailUrl { get; set; }
    int StockingQuantity { get; set; }
    bool IsDeleted { get; set; }
    int? BrandId { get; set; }
    int? CategoryId { get; set; }
}