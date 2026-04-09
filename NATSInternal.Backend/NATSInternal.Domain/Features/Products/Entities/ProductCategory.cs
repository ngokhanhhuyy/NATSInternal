using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Products;

internal class ProductCategory : AbstractEntity
{
    #region Constructors
#nullable disable
    private ProductCategory() { }
#nullable enable

    public ProductCategory(string name, DateTime createdDateTime)
    {
        Name = name;
        CreatedDateTime = createdDateTime;
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime? LastUpdatedDateTime { get; private set; }
    #endregion

    #region Methods
    public void Update(string name, DateTime updatedDateTime)
    {
        Name = name;
        LastUpdatedDateTime = updatedDateTime;
    }
    #endregion
}