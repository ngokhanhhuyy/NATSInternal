using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public class ProductCategoryGetDetailResponseDto
{
    #region Constructors
    internal ProductCategoryGetDetailResponseDto(ProductCategory brand, User? createdUser)
    {
        Id = brand.Id;
        Name = brand.Name;
        CreatedUser = new(createdUser);
        CreatedDateTime = brand.CreatedDateTime;
    }

    internal ProductCategoryGetDetailResponseDto(
        ProductCategory brand,
        User? createdUser,
        User? lastUpdatedUser) : this(brand, createdUser)
    {
        LastUpdatedUser = new(lastUpdatedUser);
        LastUpdatedDateTime = brand.LastUpdatedDateTime;
    }
    #endregion

    #region Properties 
    public Guid Id { get; }
    public string Name { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    #endregion
}